using System;
using System.Data;
using Brochure.ORM.Database;

namespace Brochure.ORM
{
    public abstract class DbOption
    {
        public IsolationLevel TransactionLevel { get; set; } = IsolationLevel.ReadUncommitted;
        public string ConnectionString { get; set; }
        public int Timeout { get; set; }
        public string DatabaseName { get; set; }
        public bool IsUseParamers { get; set; } = true;
    }
}