using IGNQuery.Interfaces.QueryProvider;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGNQuery.SqlServer
{
    internal class MSSQLCreateQuery : ICreateQuery
    {
        private string _query;

        public MSSQLCreateQuery(string query)
        {
            _query = query;
        }

        public string GetResultingString()
        {
            return _query;
        }

        public IQueryResult StoredProcedureIfNotExists(string name, IQueryResult content, IEnumerable<TableField> parameters = null)
        {
            var query = new StringBuilder();
            query.Append("IF OBJECT_ID('");
            query.Append(name);
            query.Append("', 'P') IS NOT NULL");
            query.AppendLine();
            query.Append("DROP PROCEDURE ");
            query.Append(name);
            query.Append("GO");
            query.AppendLine();
            query.Append("CREATE STORED PROCEDURE ");
            query.Append(name);
            query.Append(" ");
            if (parameters != null)
            {
                foreach (var col in parameters)
                {
                    query.Append(col.Name);
                    query.Append(" ");
                    query.Append(col.Type);
                    query.Append(", ");
                }
            }
            query.AppendLine();
            query.Append(" AS ");
            query.Append(content);
            query.Append("GO");
            _query += query;
            return new QueryResult(_query);
        }

        public IQueryResult TableIfNotExists(string name, IEnumerable<TableField> fields)
        {
            var query = new StringBuilder();
            query.Append("IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='");
            query.Append(name);
            query.Append("' and xtype='U')");
            query.AppendLine();
            query.Append("CREATE TABLE ");
            query.Append(name);
            query.Append("(");
            var last = fields.LastOrDefault();
            foreach (var col in fields)
            {
                query.Append(col.Name);
                query.Append(" ");
                query.Append(col.Type);
                query.Append(" ");
                query.Append(col.CanHaveNull ? "null" : "not null");
                if (!string.IsNullOrEmpty(col.DefValue))
                {
                    query.Append(" default ");
                    query.Append(col.DefValue);
                }
                if (col.Generated)
                {
                    query.Append(" IDENTITY(1,1)");
                }
                if (col != last)
                {
                    query.Append(", ");
                }
            }
            query.Append(")");

            _query += query;

            return new QueryResult(_query);
        }
    }
}
