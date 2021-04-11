using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Brochure.Extensions;
using Brochure.ORM.Visitors;
namespace Brochure.ORM
{
    public abstract class Query : IQuery, IWhereQuery
    {
        private readonly DbOption option;

        protected List<IDbDataParameter> DbParameters;

        protected Query(IDbProvider dbProvider, DbOption option, IVisitProvider visitProvider)
        {
            this.option = option;
            InitData();
            this.dbProvider = dbProvider;
            this.visitProvider = visitProvider;
        }

        private void InitData()
        {
            DbParameters = new List<IDbDataParameter>();
            InitMainTableNames();
        }

        protected string selectSql;
        protected string whereSql;

        protected List<string> mainTableNames;

        protected string groupSql;
        protected string joinSql;
        protected string orderSql;
        protected List<IDbDataParameter> parameters;
        private readonly IDbProvider dbProvider;
        private readonly IVisitProvider visitProvider;

        private string AddWhiteSpace(string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
                return str + " ";
            return string.Empty;
        }

        // public int Count ()
        // {
        //     selectSql = $"select count(1) from {JoinTableNames()}";
        //     var sql = GetSql ();
        //     var result = -1;
        //     using (var conn = this.option.GetDbConnection ())
        //     {
        //         var command = conn.CreateCommand ();
        //         command.CommandText = sql;
        //         command.CommandTimeout = this.option.Timeout;
        //         if (option.IsUseParamers)
        //         {
        //             foreach (var item in parameters)
        //                 command.Parameters.Add (item);
        //         }
        //         result = command.ExecuteNonQuery ();
        //     }
        //     return result;
        // }

        protected T Join<T>(Type type, Expression expression) where T : Query
        {
            var joinVisitor = visitProvider.Builder<JoinVisitor>();
            joinVisitor.SetTableName(type);
            joinVisitor.Visit(expression);
            string t_joinSql;
            if (string.IsNullOrWhiteSpace(joinSql))
                t_joinSql = $"{joinVisitor.GetSql()}";
            else
                t_joinSql = $"{joinSql} {joinVisitor.GetSql()}";
            var tt = this.Copy<T>();
            tt.joinSql = t_joinSql;
            return tt;
        }

        protected T OrderBy<T>(Expression fun) where T : Query
        {
            var orderVisitor = visitProvider.Builder<OrderVisitor>();
            orderVisitor.Visit(fun);
            var t_orderSql = orderVisitor.GetSql()?.ToString() ?? string.Empty;
            var tt = this.Copy<T>();
            tt.orderSql = t_orderSql;
            return tt;
        }
        protected T OrderBy<T>(string str) where T : Query
        {
            this.orderSql = str;
            return (T)this;
        }

        protected T OrderByDesc<T>(Expression fun) where T : Query
        {
            var orderVisitor = visitProvider.Builder<OrderVisitor>();
            orderVisitor.IsAes = false;
            orderVisitor.Visit(fun);
            var t_orderSql = orderVisitor.GetSql()?.ToString() ?? string.Empty;
            var tt = this.Copy<T>();
            tt.orderSql = t_orderSql;
            return tt;
        }
        protected T OrderByDesc<T>(string str) where T : Query
        {
            this.orderSql = str;
            return (T)this;
        }
        protected T Select<T>(Expression fun) where T : Query
        {
            var selectVisitor = visitProvider.Builder<SelectVisitor>();
            selectVisitor.Visit(fun);
            var t_selectSql = selectVisitor.GetSql()?.ToString() ?? string.Empty;
            //todo 此处如果FormateField 包含 正则表达式的特殊字符串 则需要进行转义
            //如果有group 则需要将Key替换成对应的分组属性
            if (t_selectSql.ContainsReg($@"{dbProvider.FormatFieldName(@"<>f__AnonymousType[0-9]`[0-9]")}") && !string.IsNullOrWhiteSpace(groupSql))
            {
                var groupField = groupSql.Replace("group by", "").Trim();
                var groupFields = groupField.Split(',');
                foreach (var item in groupFields)
                {
                    var filed = item.Split('.')[1];
                    t_selectSql = t_selectSql.ReplaceReg($@"{dbProvider.FormatFieldName(@"<>f__AnonymousType[0-9]`[0-9]")}.{filed}", item);
                }
            }
            if (t_selectSql.ContainsReg($@"{dbProvider.FormatFieldName(@"IGrouping`[0-9]")}.{dbProvider.FormatFieldName(@"Key")}") && !string.IsNullOrWhiteSpace(groupSql))
            {
                var groupField = groupSql.Replace("group by", "").Trim();

                t_selectSql = t_selectSql.ReplaceReg($@"{dbProvider.FormatFieldName(@"IGrouping`[0-9]")}.{dbProvider.FormatFieldName(@"Key")}", groupField);
            }
            t_selectSql += JoinTableNames();
            T tt;
            if (this is T t)
                tt = t;
            else
                tt = this.Copy<T>();
            tt.selectSql = t_selectSql;
            return tt;
        }

        protected T WhereAnd<T>(Expression fun) where T : Query
        {
            var whereVisitor = visitProvider.BuilderNew<WhereVisitor>();
            whereVisitor.AddParamters(this.DbParameters);
            whereVisitor.Visit(fun);
            var sql = whereVisitor.GetSql()?.ToString() ?? string.Empty;
            string t_whereSql = string.Empty;
            if (string.IsNullOrWhiteSpace(whereSql))
            {
                t_whereSql = sql;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(sql))
                {
                    t_whereSql = $"{whereSql} and ({sql.Replace("where ", "")})";
                }
            }
            var parameters = whereVisitor.GetParameters();
            var tt = this.Copy<T>();
            tt.whereSql = t_whereSql;
            tt.DbParameters = parameters.ToList();
            return tt;
        }
        public T WhereAnd<T>(string str) where T : Query
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return (T)this;
            }
            string t_whereSql;
            if (string.IsNullOrWhiteSpace(whereSql))
            {
                t_whereSql = str;
            }
            else
            {
                t_whereSql = $"{whereSql} and ({str})";
            }
            this.whereSql = t_whereSql;
            return (T)this;
        }

        public T WhereOr<T>(string str) where T : Query
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return (T)this;
            }
            string t_whereSql;
            if (string.IsNullOrWhiteSpace(whereSql))
            {
                t_whereSql = str;
            }
            else
            {
                t_whereSql = $"{whereSql} or ({str})";
            }
            this.whereSql = t_whereSql;
            return (T)this;
        }
        protected T WhereOr<T>(Expression fun) where T : Query
        {
            var whereVisitor = visitProvider.BuilderNew<WhereVisitor>();
            whereVisitor.AddParamters(this.DbParameters);
            whereVisitor.Visit(fun);
            var sql = whereVisitor.GetSql()?.ToString() ?? string.Empty;
            string t_whereSql = string.Empty;
            if (string.IsNullOrWhiteSpace(whereSql))
            {
                t_whereSql = sql;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(sql))
                    t_whereSql = $"{whereSql} or ({sql.Replace("where ", "")})";
            }
            var parameters = whereVisitor.GetParameters();
            var tt = this.Copy<T>();
            tt.whereSql = t_whereSql;
            tt.DbParameters = parameters.ToList();
            return tt;
        }

        protected T Groupby<T>(Expression fun) where T : Query
        {
            var groupVisit = visitProvider.Builder<GroupVisitor>();
            groupVisit.Visit(fun);
            var t_groupSql = groupVisit.GetSql().ToString();
            var tt = this.Copy<T>();
            tt.groupSql = t_groupSql;
            return tt;
        }

        protected T Copy<T>() where T : Query
        {
            var query = (T)Activator.CreateInstance(typeof(T), this.dbProvider, this.option, this.visitProvider);
            query.selectSql = this.selectSql;
            query.whereSql = this.whereSql;
            query.groupSql = this.groupSql;
            query.joinSql = this.joinSql;
            query.orderSql = this.orderSql;
            query.parameters = this.parameters;
            query.mainTableNames = this.mainTableNames;
            query.DbParameters = new List<IDbDataParameter>(this.DbParameters);
            return query;
        }

        public string GetSql()
        {
            if (string.IsNullOrWhiteSpace(selectSql))
            {
                selectSql = $"select * from {JoinTableNames()}";
            }
            return $"{AddWhiteSpace(selectSql)}{AddWhiteSpace(joinSql)}{AddWhiteSpace(groupSql)}{AddWhiteSpace(whereSql)}{AddWhiteSpace(orderSql)}".Trim();
        }

        public string GetWhereSql()
        {
            return AddWhiteSpace(whereSql);
        }

        public List<IDbDataParameter> GetDbDataParameters()
        {
            return DbParameters;
        }

        protected string JoinTableNames()
        {
            return string.Join(",", mainTableNames.Select(t => $"{dbProvider.FormatFieldName(t)}"));
        }

        private void InitMainTableNames()
        {
            mainTableNames = new List<string>();
            var thisTypes = this.GetType();
            var types = thisTypes.GetGenericArguments();
            foreach (var item in types)
            {
                this.mainTableNames.Add(TableUtlis.GetTableName(item));
            }
        }
    }
}