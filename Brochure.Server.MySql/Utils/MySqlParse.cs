using Brochure.Core;
using Brochure.Core.Server;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Brochure.Server.MySql
{
    public class MySqlParse : ISqlParse
    {
        public IDbParams Parse(ParseTree tree)
        {
            var root = tree.Root;
            return ToQuery(root);
        }

        private IDbParams ToQuery(ParseTreeNode node)
        {

            var sqlParams = new MySqlDbParams();
            sqlParams.Params = new Record();
            var name = node.Term.Name;
            if (name == "LogicExp")
            {
                var nodes = node.ChildNodes.ToList();
                var leftNode = nodes[0];
                var operaNode = nodes[1];
                var rightNode = nodes[2];
                var leftparam = ToQuery(leftNode);
                var operaparam = ToQuery(operaNode);
                var rightparam = ToQuery(rightNode);
                sqlParams.Params.Merge(leftparam.Params, false);
                sqlParams.Params.Merge(operaparam.Params, false);
                sqlParams.Params.Merge(rightparam.Params, false);
                sqlParams.Sql = $"{leftparam.Sql} {operaparam.Sql} ({rightparam.Sql})";
                // foreach (var item in nodes)
                // {
                //     var cparam = ToQuery (item);
                //     sqlParams.Params.Merge (cparam.Params, false);
                //     sqlParams.Sql = $"{sqlParams.Sql} ({cparam.Sql})";
                // }
            }
            else if (name == "LogicOper")
            {
                sqlParams.Sql = sqlParams.Sql + GetLogicOperation(node);
            }
            else
            {
                IDbParams pp = null;
                switch (name)
                {
                    case "In":
                        pp = GetInParams(node);
                        break;
                    case "NotIn":
                        pp = GetNotInParams(node);
                        break;
                    case "Between":
                        pp = GetBetweenParams(node);
                        break;
                    case "NotBetween":
                        pp = GetNotBetweenParams(node);
                        break;
                    case "Like":
                        pp = GetLikeParams(node);
                        break;
                    case "NotLike":
                        pp = GetNotLikeParams(node);
                        break;
                    case "NotNull":
                        pp = GetNotNullParams(node);
                        break;
                    case "Null":
                        pp = GetNullParams(node);
                        break;
                    case "OperationExp":
                        pp = GetOperationExp(node);
                        break;
                }
                if (pp != null)
                {
                    sqlParams.Params.Merge(pp.Params, false);
                    sqlParams.Sql = sqlParams.Sql + pp.Sql;
                }
            }
            return sqlParams;
        }
        private string GetLogicOperation(ParseTreeNode node)
        {
            var str = string.Empty;
            var name = node.Term.Name;
            var opearName = node.ChildNodes[0].Token.ValueString;
            switch (opearName)
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
        private IDbParams GetOperationExp(ParseTreeNode node)
        {
            var left = node.ChildNodes[0];
            var opera = node.ChildNodes[1];
            var right = node.ChildNodes[2];
            var field = left.Token.ValueString;
            var sqlParams = GetParams(field, right);
            sqlParams.Sql = $"{field} {opera.Token.ValueString} {string.Join(",", sqlParams.Params.Keys.ToList())}";
            return sqlParams;
        }
        private IDbParams GetInParams(ParseTreeNode node)
        {
            // in ('' ,'','')
            var left = node.ChildNodes[0];
            var right = node.ChildNodes[2];
            var field = left.Token.ValueString;
            var sqlParams = GetParams(field, right.ChildNodes.ToList());
            sqlParams.Sql = $"{field} in ({string.Join(",", sqlParams.Params.Keys.ToList())})";
            return sqlParams;
        }

        private IDbParams GetNotInParams(ParseTreeNode node)
        {
            // in ('' ,'','')
            var left = node.ChildNodes[0];
            var right = node.ChildNodes[2];
            var field = left.Token.ValueString;
            var sqlParams = GetParams(field, right.ChildNodes.ToList());
            sqlParams.Sql = $"{field} not in ({string.Join(",", sqlParams.Params.Keys.ToList())})";
            return sqlParams;
        }

        private IDbParams GetBetweenParams(ParseTreeNode node)
        {
            // betwween {1} and {2}
            var left = node.ChildNodes[0];
            var field = left.Token.ValueString;
            var starField = node.ChildNodes[2];
            var endField = node.ChildNodes[4];
            var sqlParams = GetParams(field, new ParseTreeNodeList() { starField, endField });
            sqlParams.Sql = $"{field} between {sqlParams.Params.Keys.ToList()[0]} and {sqlParams.Params.Keys.ToList()[1]}";
            return sqlParams;
        }

        private IDbParams GetNotBetweenParams(ParseTreeNode node)
        {
            // betwween {1} and {2}
            var left = node.ChildNodes[0];
            var field = left.Token.ValueString;
            var starField = node.ChildNodes[2];
            var endField = node.ChildNodes[4];
            var sqlParams = GetParams(field, new ParseTreeNodeList() { starField, endField });
            sqlParams.Sql = $"{field} not between {sqlParams.Params.Keys.ToList()[0]} and {sqlParams.Params.Keys.ToList()[1]}";
            return sqlParams;
        }

        private IDbParams GetLikeParams(ParseTreeNode node)
        {
            var left = node.ChildNodes[0];
            var right = node.ChildNodes[2];
            var field = left.Token.ValueString;
            var sqlParams = GetParams(field, right);
            sqlParams.Sql = $"{field} like '%{string.Join(",", sqlParams.Params.Keys.ToList())}%'";
            return sqlParams;
        }
        private IDbParams GetNotLikeParams(ParseTreeNode node)
        {
            var left = node.ChildNodes[0];
            var right = node.ChildNodes[2];
            var field = left.Token.ValueString;
            var sqlParams = GetParams(field, right);
            sqlParams.Sql = $"{field} not like '%{string.Join(",", sqlParams.Params.Keys.ToList())}%'";
            return sqlParams;
        }
        private IDbParams GetNotNullParams(ParseTreeNode node)
        {
            var left = node.ChildNodes[0];
            var field = left.Token.ValueString;
            var sqlParams = new MySqlDbParams();
            sqlParams.Sql = $"{field} is not null";
            return sqlParams;
        }

        private IDbParams GetNullParams(ParseTreeNode node)
        {
            var left = node.ChildNodes[0];
            var field = left.Token.ValueString;
            var sqlParams = new MySqlDbParams();
            sqlParams.Sql = $"{field} is null";
            return sqlParams;
        }

        private IDbParams GetParams(string paramsName, List<ParseTreeNode> nodes)
        {
            var sqlParams = new MySqlDbParams();
            var count = 0;
            foreach (var item in nodes)
            {
                count++;
                sqlParams.Params.Merge(GetValuesParams(paramsName + count, sqlParams.ParamSymbol, item), false);
            }
            return sqlParams;
        }

        private IDbParams GetParams(string paramsName, ParseTreeNode node)
        {
            var sqlParams = new MySqlDbParams();
            sqlParams.Params.Merge(GetValuesParams(paramsName, sqlParams.ParamSymbol, node), false);
            return sqlParams;
        }
        private IRecord GetValuesParams(string paramsName, string symbol, ParseTreeNode node)
        {
            var param = new Record();
            if (node != null)
            {
                if (node.Term.Name == "StringValue")
                    param.Add($"{symbol}{paramsName}", node.Token.ValueString);
                else if (node.Term.Name == "NumberValue")
                    param.Add($"{symbol}{paramsName}", node.Token.Value);
                else if (node.Term.Name == "DateTimeValue")
                    param.Add($"{symbol}{paramsName}", node.Token.Value.As<DateTime>());
            }
            return param;
        }

    }
}
