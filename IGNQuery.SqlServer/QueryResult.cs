using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;

namespace IGNQuery.SqlServer
{
    internal class QueryResult : IQueryResult
    {
        private readonly string _query;
        private readonly string email;
        private IDataDriver dataDriver;

        public QueryResult(string query, string email, IDataDriver dataDriver)
        {
            _query = query;
            this.email = email;
            this.dataDriver = dataDriver;
        }

        public IGNQueriable AsIgnQueriable()
        {
            return IGNQueriable.FromQueryString(this._query, this.email, this.dataDriver);
        }

        public string GetResultingString()
        {
            return _query;
        }
    }
}
