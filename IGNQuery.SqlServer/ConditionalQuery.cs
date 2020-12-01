using IGNQuery.Interfaces.QueryProvider;

namespace IGNQuery.SqlServer
{
    internal class ConditionalQuery : IConditionalQuery
    {
        private string _query;

        public ConditionalQuery(string query)
        {
            _query = query;
        }

        public string GetResultingString()
        {
            return _query;
        }

        public ICondition Where()
        {
            _query += "WHERE ";
            return new Condition(_query);
        }
    }
}