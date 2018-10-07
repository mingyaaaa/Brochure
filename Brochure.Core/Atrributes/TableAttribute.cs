using System;
namespace Brochure.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TableAttribute : Attribute
    {
        public string Name = "";
        public TableAttribute(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException("参数错误，表明不能为空");
            Name = tableName;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class LengthAttribute : Attribute
    {
        public int Length = 0;
        public LengthAttribute(int length)
        {
            Length = 0;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class NotNullAttribute : Attribute { }
}
