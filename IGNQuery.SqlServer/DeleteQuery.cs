using IGNQuery.Interfaces.QueryProvider;

namespace IGNQuery.SqlServer
{
    internal class DeleteQuery : IDeleteQuery
    {
        private string _query;

        public DeleteQuery(string query)
        {
            _query = query;
        }

        public IQueryResult From(string table)
        {
            _query += $"DELETE FROM {table} GO";
            return new QueryResult(_query);
        }

        public IConditionalQuery FromWithCondition(string table)
        {
            _query += $"DELETE FROM {table} ";
            return new ConditionalQuery(_query);
        }

        public string GetResultingString()
        {
            return _query;
        }
    }
}