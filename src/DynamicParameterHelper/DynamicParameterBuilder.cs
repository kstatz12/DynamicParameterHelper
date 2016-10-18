using System.Linq;
using System.Reflection;
using Dapper;
using DynamicParameterHelper.Attributes;
using DynamicParameterHelper.Enums;

namespace DynamicParameterHelper
{
    public static class DynamicParameterBuilder
    {
        public static DynamicParameters ToDynamicParameters(this object obj, CrudType type)
        {
            var parameters = new DynamicParameters();
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttributes(typeof(DynamicParameterAttribute), true).FirstOrDefault() as DynamicParameterAttribute;
                if (attribute == null) continue;
                switch (type)
                {
                    case CrudType.Create:
                        if (!attribute.IsIdentity)
                        {
                            parameters.Add(attribute.ParameterName, property.GetValue(obj, null), attribute.Type, size: attribute.Scalar);
                        }
                        break;
                    default:
                        parameters.Add(attribute.ParameterName, property.GetValue(obj, null), attribute.Type, size: attribute.Scalar);
                        break;
                }
            }
            return parameters;
        } 
    }
}