using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;
using System.Collections.Generic;

namespace IGNQuery.SqlServer
{
    internal class SelectQuery : ISelectQuery
    {
        private string _query;
        private readonly string email;
        private IDataDriver dataDriver;

        public SelectQuery(string query, string email, IDataDriver dataDriver)
        {
            _query = query;
            this.email = email;
            this.dataDriver = dataDriver;
        }

        public IQueryResult AllFrom(string table)
        {
            _query += $"SELECT * FROM {table} ";
            return new QueryResult(_query, this.email, this.dataDriver);
        }

        public IConditionalQuery AllFromWithCondition(string table)
        {
            _query += $"SELECT * FROM {table} ";
            return new ConditionalQuery(_query, this.email, this.dataDriver);
        }

        public IGNQueriable AsIgnQueriable()
        {
            throw new System.NotImplementedException();
        }

        public IQueryResult FieldsFrom(string table, IEnumerable<string> fieldNames)
        {
            _query += $"SELECT {string.Join(",", fieldNames)} FROM {table} ";
            return new QueryResult(_query, this.email, this.dataDriver);
        }

        public IConditionalQuery FieldsFromWithCondition(string table, IEnumerable<string> fieldNames)
        {
            _query += $"SELECT {string.Join(",", fieldNames)} FROM {table} ";
            return new ConditionalQuery(_query, this.email, this.dataDriver);
        }

        public string GetResultingString()
        {
            return _query;
        }
    }
}