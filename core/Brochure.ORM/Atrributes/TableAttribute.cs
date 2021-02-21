using System;
namespace Brochure.ORM
{
    [AttributeUsage (AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class TableAttribute : Attribute
    {
        public string Name = "";

        public TableAttribute (string tableName)
        {
            if (string.IsNullOrWhiteSpace (tableName))
                throw new ArgumentException ("参数错误，表明不能为空");
            Name = tableName;
        }
    }
}