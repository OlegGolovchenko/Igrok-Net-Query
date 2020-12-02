using IGNQuery.Interfaces.QueryProvider;
using System.Text;

namespace IGNQuery.SqlServer
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
            query.Append("IF OBJECT_ID('");
            query.Append(name);
            query.Append("', 'P') IS NOT NULL");
            query.AppendLine();
            query.Append("DROP PROCEDURE ");
            query.Append(name);
            query.Append("GO");
            _query += query.ToString();
            return new QueryResult(_query);
        }

        public IQueryResult TableIfExists(string name)
        {
            var query = new StringBuilder();
            query.Append("IF OBJECT_ID('");
            query.Append(name);
            query.Append("', 'U') IS NOT NULL");
            query.AppendLine();
            query.Append("DROP PROCEDURE ");
            query.Append(name);
            query.Append("GO");
            _query += query.ToString();
            return new QueryResult(_query);
        }
    }
}
