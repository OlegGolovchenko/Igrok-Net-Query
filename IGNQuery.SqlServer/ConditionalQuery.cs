using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;

namespace IGNQuery.SqlServer
{
    internal class ConditionalQuery : IConditionalQuery
    {
        private string _query;
        private readonly string email;
        private IDataDriver dataDriver;

        public ConditionalQuery(string query, string email, IDataDriver dataDriver)
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

        public ICondition Where()
        {
            _query += "WHERE ";
            return new Condition(_query, this.email, this.dataDriver);
        }
    }
}