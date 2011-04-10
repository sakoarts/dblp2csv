using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autofac;
using DblpDqSimulator.Common;
using DqMetricSimulator.Core;
using DqMetricSimulator.Data;
using DqMetricSimulator.Simulate;

namespace DblpDqSimulator
{
    public partial class frmGen : Form
    {
        public frmGen()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            var container = Bootstrapper.Get();
            var dal = container.Resolve<Dal>();

            const string query = @"
SELECT  p.Id, r.Id, p.title, r.conference, r.title, r.publisher, r.year
FROM papers p
INNER JOIN Raw_Links l
ON p.Id = l.[From]
INNER JOIN proceedings r
ON l.[To] = r.Id
AND l.[link-type]='in-proceedings'
";
            //Select all conferences.
            var dt = dal.GetDataTable(Dal.DefaultConnStr, query);
            dt.PrimaryKey = new[] {dt.Columns[0], dt.Columns[1]}; // Setting the primary key is necessary
            var tbl = ImportDataTable.ImportAdoNetDataTable(dt);
            var baseHist = SimulationBuilder.GetNormalDistributedBaseHistogram();
            
            //Now we have the base histogram.
            //We want to generate a histogram for a column and save it back to database.
            //Let's select conference
            const int selectedCol = 3;
            const int metricId = 1;
            var hist = SimulationBuilder.GetHistogram(tbl.Columns[selectedCol] as Column<string>, baseHist);
            var time = new Random(DateTime.Now.Second);
            var loockup = SimulationBuilder.CreateSimulatedLoockup(tbl, selectedCol, hist,
                                                              r => TimeSpan.FromMilliseconds(time.Next(3000)));
            //Now we've got the loockup. Store it somewehere. We put it back in the database.
            //We already have a table called SimulatedMetricLoockup
            const string insertMetric = "INSERT INTO Metrics (Id, MetricName, OnQuery, OnColumn) VALUES (@mId, @mName, @mQuery, @mCol);";
            const string updateMetric = "UPDATE Metrics SET Id = @mId, MetricName = @mName, OnQuery = @mQuery, OnColumn =  @mCol";
            dal.InsertOrUpdateEntity(Dal.DefaultConnStr,
                                     insertMetric,
                                     updateMetric,
                                     "SELECT Count(*) FROM Metrics Where Id = @mId",
                                     new Dictionary<string, object>
                                         {
                                             {"@mId", metricId},
                                             {"@mName", "NormalDistru"},
                                             {"mQuery", query},
                                             {"mCol", selectedCol}
                                         }
                );

            //Put everything in a temp file.
            var tmpFileName = dal.InsertAllInTempFile(loockup, l => String.Format("{0},{1},{2},{3}", metricId, l.Key, l.Value.Result ? 1 : 0, l.Value.Duration.Milliseconds) );
            //bulk insert 
            string bulkIns =
                @"BULK INSERT SimulatedMetricLoockup FROM '{0}' WITH ( FIELDTERMINATOR = ',', ROWTERMINATOR='\n' )".
                    FormatWith(tmpFileName);
            dal.RunCommandWithParameter(Dal.DefaultConnStr, bulkIns);
            //const string insertLoockupQ = @"INSERT INTO SimulatedMetricLoockup (MetricId, LoockupKey, LoockupValue, Delay) VALUES (@mId, @lKey, @lVal, @lD)";
            //dal.RunForAll(Dal.DefaultConnStr, insertLoockupQ, loockup.ToList(),
            //              l => new Dictionary<string, object> {{"@mId", 1}, {"@lKey", l.Key}, {"@lVal", l.Value.Result}, {"@lD", l.Value.Duration}}
            //    );
        }
    }
}
