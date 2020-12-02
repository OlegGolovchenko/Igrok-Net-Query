using IGNQuery.Interfaces.QueryProvider;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IGNQuery.MySql
{
    internal class MySQLCreateQuery : ICreateQuery
    {
        private string _query;

        public MySQLCreateQuery(string query)
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
            query.Append("DROP PROCEDURE IF EXISTS ");
            query.Append(name);
            query.Append(";");
            query.AppendLine();
            query.Append("CREATE STORED PROCEDURE ");
            query.Append(name);
            query.Append("(");
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
            query.Append(")");
            query.Append(content);
            query.Append(";");
            _query += query;
            return new QueryResult(_query);
        }

        public IQueryResult TableIfNotExists(string name, IEnumerable<TableField> fields)
        {
            var query = new StringBuilder();
            query.Append("CREATE TABLE IF NOT EXISTS ");
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
                    var defValue = col.DefValue;
                    if (col.Type == "bit")
                    {
                        defValue = col.DefValue == "'true'" ? "1" : "0";
                    }
                    query.Append(defValue);
                }
                if (col.Generated)
                {
                    query.Append(" auto_increment");
                }
                if (col != last)
                {
                    query.Append(", ");
                }
            }
            var pk = fields.SingleOrDefault(fld => fld.Primary);
            if (pk != null)
            {
                query.Append(", constraint ");
                query.Append("pk_");
                query.Append(name);
                query.Append(pk.Name);
                query.Append(" primary key(");
                query.Append(pk.Name);
                query.Append(")");
            }
            query.Append(");");

            _query += query;

            return new QueryResult(_query);
        }
    }
}
