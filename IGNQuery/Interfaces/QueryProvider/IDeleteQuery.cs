namespace IGNQuery.Interfaces.QueryProvider
{
    public interface IDeleteQuery : IQueryResult
    {
        IQueryResult From(string table);

        IConditionalQuery FromWithCondition(string table);
    }
}
