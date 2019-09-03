using System;

namespace TurtBOT
{
    public static class Utils
    {
        public static string TypeName(Type type)
        {
            if (type == typeof(double)) { return "Decimal"; }
            if (type == typeof(int)) { return "Integer"; }

            return type.Name;
        }
    }
}