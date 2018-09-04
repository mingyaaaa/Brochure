﻿using Irony.Parsing;
using System;

namespace Brochure.Core.Server
{
    public class QueryParse
    {
        private ISqlParse _parse;
        public QueryParse(ISqlParse parse)
        {
            _parse = parse;
        }
        public IDbParams Parse(Query query)
        {
            var language = new LanguageData(new SqlGrammer());
            var parse = new Parser(language);
            var tree = parse.Parse(query.ToString());
            if (tree.HasErrors())
            {
                throw new Exception(tree.ParserMessages.ToString());
            }
            return _parse.Parse(tree);
        }
    }
}
