using IGNQuery.Interfaces.QueryProvider;

namespace IGNQuery.MySql
{
    internal class MySQLAlterQuery : IAlterQuery
    {
        private string _query;
        private bool _isDrop;

        public MySQLAlterQuery(string query)
        {
            _query = query;
        }

        public IAlterQuery Add()
        {
            _isDrop = false;
            _query += $"add column ";
            return new MySQLAlterQuery(_query);
        }

        public IAlterQuery Alter()
        {
            _isDrop = false;
            _query += "alter column ";
            return new MySQLAlterQuery(_query);
        }

        public IAlterQuery Column(TableField column)
        {
            if (_isDrop)
            {
                _query += $"{column.Name}";
            }
            else
            {
                _query += $"{column.Name} {column.Type}";
            }
            return new MySQLAlterQuery(_query);
        }

        public IAlterQuery Drop()
        {
            _isDrop = true;
            _query += "drop column ";
            return new MySQLAlterQuery(_query);
        }

        public IAlterQuery Go()
        {
            _query += ";";
            return new MySQLAlterQuery(_query);
        }

        public IAlterQuery Next()
        {
            _query += ",";
            return new MySQLAlterQuery(_query);
        }

        public IAlterQuery TableIfExists(string tableName)
        {
            _query += $"alter table {tableName} ";
            return new MySQLAlterQuery(_query);
        }
    }
}
