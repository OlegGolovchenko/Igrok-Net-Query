namespace IGNQuery.Interfaces.QueryProvider
{
    public interface IUpdateQuery : IQueryResult
    {
        IUpdateQuery Table(string table);

        IQueryResult SetStringField(string field, string value);

        IConditionalQuery SetStringFieldWithCondition(string field, string value);

        IQueryResult SetLongField(string field, long value);

        IConditionalQuery SetLongFieldWithCondition(string field, long value);

        IQueryResult SetBooleanFieldTrue(string field);

        IConditionalQuery SetBooleanFieldTrueWithCondition(string field);

        IQueryResult SetBooleanFieldFalse(string field);

        IConditionalQuery SetBooleanFieldFalseWithCondition(string field);
    }
}
