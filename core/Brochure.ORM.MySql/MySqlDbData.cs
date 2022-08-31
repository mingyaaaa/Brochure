using AspectCore.Abstractions.DependencyInjection;
using Brochure.ORM.Atrributes;
using Brochure.ORM.Database;
using Brochure.ORM.Utils;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System.Data;
using System.Drawing;
using System.Text;

namespace Brochure.ORM.MySql
{
    /// <summary>
    /// The my sql db data.
    /// </summary>
    public class MySqlDbData : DbData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbData"/> class.
        /// </summary>
        /// <param name="dbContext"></param>
        [InjectConstructor]
        public MySqlDbData(DbContext dbContext) : base(dbContext) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbData"/> class.
        /// </summary>
        /// <param name="isBeginTransaction">If true, is begin transaction.</param>
        public MySqlDbData(bool isBeginTransaction = false) : base(new MySqlDbContext(isBeginTransaction))
        {
        }

        public override Task<int> InsertManyAsync<T>(IEnumerable<T> objs)
        {
            var sql = new InsertManySql(objs);
            return _dbContext.ExcuteNoQueryAsync(sql);
        }

        [Transaction]
        public override async Task<int> SqlBulkCopyAsync<T>(IEnumerable<T> objs)
        {
            var connect = _dbContext.Connection;

            if (connect is not MySqlConnection connection)
            {
                return -1;
            }
            if (connect.State == ConnectionState.Closed)
            {
                await connect.OpenAsync();
            }
            var tableName = TableUtlis.GetTableName<T>();
            var path = await ToCsv(objs);
            MySqlBulkLoader bulk = new MySqlBulkLoader(connection)
            {
                FieldTerminator = ",",
                FieldQuotationCharacter = '"',
                EscapeCharacter = '"',
                LineTerminator = "\r\n",
                FileName = path,
                NumberOfLinesToSkip = 0,
                TableName = tableName,    //也是mysql内表的名
            };
            return await bulk.LoadAsync();
        }

        private async Task<string> ToCsv<T>(IEnumerable<T> objs)
        {
            //以半角逗号（即,）作分隔符，列为空也要表达其存在。
            //列内容如存在半角逗号（即,）则用半角引号（即""）将该字段值包含起来。
            //列内容如存在半角引号（即"）则应替换成半角双引号（""）转义，并用半角引号（即""）将该字段值包含起来。
            StringBuilder sb = new StringBuilder();
            foreach (object row in objs)
            {
                if (row == null)
                    throw new Exception("'数据集中存在空值");
                var record = EntityUtil.AsTableRecord(row);
                foreach (var item in record)
                {
                    var obj = item.Value;
                    if (obj is string objStr && objStr.Contains(","))
                    {
                        sb.Append("\"" + objStr.Replace("\"", "\"\"") + "\"");
                    }
                    else
                    {
                        sb.Append(obj.ToString());
                    }
                }
                sb.AppendLine();
            }
            var tempPath = Path.GetTempFileName();

            StreamWriter sw = new StreamWriter(tempPath, false, UTF8Encoding.UTF8);  //要与mysql的编码方式对象, 数据库要utf8, 表也一样
            await sw.WriteAsync(sb);
            return tempPath;
        }
    }
}