using SolidWorksSecDev.Entity;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SolidWorksSecDev
{
    public static class ExtensionMethods
    {
        public static string MyToString(this SWEntity entity)
        {
            if (entity == null) return "null";
            FieldInfo[] fields = entity.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (fields.Length <= 0) return "";
            StringBuilder sb = new StringBuilder();
            foreach (FieldInfo field in fields)
            {
                sb.Append(field.Name);
                sb.Append(" : ");
                sb.Append(field.GetValue(entity));
                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
}
