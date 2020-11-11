namespace IGNQuery.Interfaces.QueryProvider
{
    public interface IConditionalQuery : IQueryResult
    {
        ICondition Where();
    }

    public interface ICondition
    {
        ICondition Field(string name);

        ICondition StringEqual(string value);

        ICondition StringEqualToParam(int paramNb);

        ICondition LongEqual(long value);

        ICondition LongEqualToParam(int paramNb);

        ICondition BoolEqual(bool value);

        ICondition BoolEqualToParam(int paramNb);

        ICondition StringNotEqual(string value);

        ICondition StringNotEqualToParam(int paramNb);

        ICondition LongNotEqual(long value);

        ICondition LongNotEqualToParam(int paramNb);

        ICondition BoolNotEqual(bool value);

        ICondition BoolNotEqualToParam(int paramNb);

        ICondition Or();

        ICondition And();

        IQueryResult Go();
    }
}
