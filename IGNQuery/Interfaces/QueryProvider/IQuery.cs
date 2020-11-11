namespace IGNQuery.Interfaces.QueryProvider
{
    public interface IQuery : IQueryResult
    {
        ICreateQuery Create();

        ISelectQuery Select();

        IDeleteQuery Delete();

        IUpdateQuery Update();

        IInsertQuery Insert();

        IDropQuery Drop();
    }
}
