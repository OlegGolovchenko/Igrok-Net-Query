namespace IGNQuery
{
    public class ParameterValue
    {
        public int ParamNumber { get; private set; }

        public string ParamValue { get; private set; }

        public ParameterValue(int number)
        {
            ParamNumber = number;
        }

        public ParameterValue(int number, string value) : this(number)
        {
            SetStringValue(value);
        }
        public ParameterValue(int number, long value) : this(number)
        {
            SetLongValueValue(value);
        }
        public ParameterValue(int number, bool value) : this(number)
        {
            SetBooleanValue(value);
        }

        private void SetStringValue(string value)
        {
            ParamValue = $"'{value}'";
        }

        private void SetLongValueValue(long value)
        {
            ParamValue = $"{value}";
        }

        private void SetBooleanValue(bool value)
        {
            ParamValue = $"{(value ? 1 : 0)}";
        }
    }
}
