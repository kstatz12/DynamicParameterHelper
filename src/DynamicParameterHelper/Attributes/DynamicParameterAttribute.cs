using System;
using System.Data;

namespace DynamicParameterHelper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DynamicParameterAttribute : Attribute
    {
        public bool IsIdentity { get; private set; }
        public string ParameterName { get; private set; }
        public DbType Type { get; private set; }
        //Named Paramter because is optional
        public int Scalar { get; set; }

        public DynamicParameterAttribute(bool isIdentity, string parameterName, DbType type)
        {
            this.IsIdentity = isIdentity;
            this.ParameterName = parameterName;
            this.Type = type;
        }
    }
}