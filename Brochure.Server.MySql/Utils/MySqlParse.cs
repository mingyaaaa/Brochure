using System;
using System.Collections.Generic;
using System.Linq;
using Brochure.Core.Extends;
using Brochure.Core.Implements;
using Brochure.Core.Interfaces;
using Brochure.Core.Querys;
using Brochure.Core.Server.Interfaces;
using Brochure.Server.MySql.Implements;
using Irony.Parsing;

namespace Brochure.Server.MySql.Utils
{
    public class MySqlParse : ISqlParse
    {
        public IDbParams Parse (ParseTree tree)
        {
            var root = tree.Root;
            return ToQuery (root.ChildNodes.ToList ());
        }

        private IDbParams ToQuery (List<ParseTreeNode> nodes)
        {
            var sqlParams = new MySqlDbParams ();
            sqlParams.Params = new Record ();
            foreach (var item in nodes)
            {
                var name = item.Term.Name;
                if (name == "LogicExp")
                {
                    var tp = ToQuery (item.ChildNodes.ToList ());
                    sqlParams.Sql = sqlParams.Sql + tp.Sql;
                    sqlParams.Params.Merge (tp.Params, false);
                    continue;
                }
                else if (name == "OperationExp")
                {

                }
                else if (name == "LogicOper")
                {
                    sqlParams.Sql = sqlParams.Sql + GetLogicOperation (item);
                }
                else
                {
                    switch (name)
                    {
                        case "In":
                            break;
                        case "Betweent":
                            break;
                        case "Like":
                            break;
                        case "NotNull":
                            break;
                        case "Null":
                            break;
                    }
                }
            }
            return sqlParams;
        }
        private string GetLogicOperation (ParseTreeNode node)
        {
            var str = string.Empty;
            var name = node.Term.Name;
            switch (name)
            {
                case QueryOperationType.And:
                    str = "and";
                    break;
                case QueryOperationType.Or:
                    str = "or";
                    break;
            }
            return str;
        }
        private IDbParams GetInParams (ParseTreeNode node)
        {
            var sqlParams = new MySqlDbParams ();
            var left = node.ChildNodes[0];
            var right = node.ChildNodes[2];
            var field = left.Token.ValueString;
            return sqlParams;
            // sqlParams.Sql = $"{field} in {}";
        }
        private IRecord GetValuesParams (string paramsName, List<ParseTreeNode> nodes)
        {
            var sqlParams = new MySqlDbParams ();
            var count = 1;
            List<object> list = new List<object> ();
            if (nodes.Count == 1)
            {

            }
            else
            {
                foreach (var item in nodes)
                {
                    if (item.Term.Name == "StringValue")
                        sqlParams.Params.Add ($"'{item.Token.ValueString}'", "");
                    if (item.Term.Name == "NumberValue")
                        list.Add (item.Token.Value);
                    if (item.Term.Name == "DateTimeValue")
                        list.Add (item.Token.Value.As<DateTime> ());
                }
            }
            return sqlParams.Params;
        }
        private IRecord GetValuesParams (string paramsName, string symbol, ParseTreeNode node)
        {
            var param = new Record ();
            if (node.Term.Name == "StringValue")
                param.Add ($"{symbol}{paramsName}", node.Token.ValueString);
            else if (node.Term.Name == "NumberValue")
                param.Add ($"{symbol}{paramsName}", node.Token.Value);
            else if (node.Term.Name == "DateTimeValue")
                param.Add ($"{symbol}{paramsName}", node.Token.Value.As<DateTime> ());
            return param;
        }
    }
}