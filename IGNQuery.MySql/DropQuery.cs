using IGNQuery.Interfaces.QueryProvider;
using System.Text;

namespace IGNQuery.MySql
{
    public class DropQuery : IDropQuery
    {
        private string _query;

        public DropQuery(string query)
        {
            _query = query;
        }

        public string GetResultingString()
        {
            return _query;
        }

        public IQueryResult StoredProcedureIfExists(string name)
        {
            var query = new StringBuilder();
            query.Append("DROP PROCEDURE IF EXISTS ");
            query.Append(name);
            query.Append(";");
            _query += query.ToString();
            return new QueryResult(_query);
        }

        public IQueryResult TableIfExists(string name)
        {
            var query = new StringBuilder();
            query.Append("DROP TABLE IF EXISTS ");
            query.Append(name);
            query.Append(";");
            _query += query.ToString();
            return new QueryResult(_query);
        }
    }
}
