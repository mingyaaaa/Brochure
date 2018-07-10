using System;
using Brochure.Core.Querys;
using Brochure.Core.Server.Interfaces;
using Irony.Parsing;

namespace Brochure.Core.Server.core
{
    public class QueryParse
    {
        private ISqlParse _parse;
        public QueryParse (ISqlParse parse)
        {
            _parse = parse;
        }
        public IDbParams Parse (Query query)
        {
            var language = new LanguageData (new SqlGrammer ());
            var parse = new Parser (language);
            var tree = parse.Parse (query.ToString ());
            if (tree.HasErrors ())
            {
                throw new Exception (tree.ParserMessages.ToString ());
            }
            return _parse.Parse (tree);
        }
    }
}