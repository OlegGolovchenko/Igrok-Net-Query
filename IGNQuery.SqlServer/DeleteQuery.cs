using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;

namespace IGNQuery.SqlServer
{
    internal class DeleteQuery : IDeleteQuery
    {
        private string _query;
        private readonly string email;
        private IDataDriver dataDriver;

        public DeleteQuery(string query, string email, IDataDriver dataDriver)
        {
            _query = query;
            this.email = email;
            this.dataDriver = dataDriver;
        }

        public IGNQueriable AsIgnQueriable()
        {
            return IGNQueriable.FromQueryString(this._query, this.email, this.dataDriver);
        }

        public IQueryResult From(string table)
        {
            _query += $"DELETE FROM {table} ";
            return new QueryResult(_query, this.email, this.dataDriver);
        }

        public IConditionalQuery FromWithCondition(string table)
        {
            _query += $"DELETE FROM {table} ";
            return new ConditionalQuery(_query, this.email, this.dataDriver);
        }

        public string GetResultingString()
        {
            return _query;
        }
    }
}