using System;


namespace DblpDqSimulator.Common
{
    public static class ObjectExtension
    {
        public static TOut IfNotNull<TIn, TOut>(this TIn theIn, Func<TIn, TOut> f) where TIn : class
        {
            return theIn != null ? f(theIn) : default(TOut);
        }

        public static string FormatWith(this string str, params object[] pars)
        {
            return String.Format(str, pars);
        }
    }
}
