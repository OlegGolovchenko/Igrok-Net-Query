namespace IGNQuery
{
    public class TableField
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool CanHaveNull { get; set; }

        public bool Generated { get; set; }

        public string DefValue { get; set; }

        public bool Primary { get; set; }

        public const string TYPE_LONG = "bigint";

        public const string TYPE_BOOLEAN = "bit";

        public static string TypeNvarchar(int length)
        {
            return $"nvarchar({length})";
        }
    }
}
