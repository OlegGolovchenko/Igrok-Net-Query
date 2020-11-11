namespace IGNQuery.Interfaces.QueryProvider
{
    public interface IDropQuery : IQueryResult
    {
        IQueryResult TableIfExists(string name);

        IQueryResult StoredProcedureIfExists(string name);
    }
}
