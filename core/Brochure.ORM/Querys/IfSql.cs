using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM.Querys
{
    /// <summary>
    /// The exist sql.
    /// </summary>
    public class IfSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IfSql"/> class.
        /// </summary>
        /// <param name="thenSql">The then sql.</param>
        /// <param name="conditionSql">The condition sql.</param>
        /// <param name="elseSql">The else sql.</param>
        /// <param name="database">The database.</param>
        public IfSql(ISql thenSql, ISql conditionSql, ISql elseSql, string database = "")
        {
            ThenSql = thenSql;
            ConditionSql = conditionSql;
            ElseSql = elseSql;
            Database = database;
        }

        /// <summary>
        /// Gets or sets the sql.
        /// </summary>
        public ISql ThenSql { get; set; }

        /// <summary>
        /// Gets or sets the condition sql.
        /// </summary>
        public ISql ConditionSql { get; set; }

        /// <summary>
        /// Gets or sets the else sql.
        /// </summary>
        public ISql ElseSql { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }
    }

    /// <summary>
    /// The string sql.
    /// </summary>
    public class StringSql : ISql
    {
        private readonly string _str;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringSql"/> class.
        /// </summary>
        /// <param name="str">The str.</param>
        public StringSql(string str)
        {
            _str = str;
        }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Tos the string.
        /// </summary>
        /// <returns>A string.</returns>
        public override string ToString()
        {
            return _str;
        }
    }

    /// <summary>
    /// The exist sql.
    /// </summary>
    public class ExistSql : ISql
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExistSql"/> class.
        /// </summary>
        /// <param name="isNot">If true, is not.</param>
        /// <param name="subSql">The sub sql.</param>
        /// <param name="database">The database.</param>
        public ExistSql(ISql subSql, bool isNot = false, string database = "")
        {
            IsNot = isNot;
            SubSql = subSql;
            Database = database;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is not.
        /// </summary>
        public bool IsNot { get; set; }

        /// <summary>
        /// Gets or sets the sub sql.
        /// </summary>
        public ISql SubSql { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        public string Database { get; set; }
    }
}