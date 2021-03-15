//#############################################
//
//  MIT License
//
//  Copyright(c) 2020 Oleg Golovchenko
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//  
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.  
//
// ############################################

using IGNQuery.Enums;

namespace IGNQuery.BaseClasses.Business
{
    public class IGNCondition
    {
        public string ColumnName { get; private set; }

        public IGNSqlCondition Operand { get; private set; }

        public object Value { get; private set; }

        internal IGNCondition(
            string columnName, 
            IGNSqlCondition operand, 
            object value
            )
        {
            this.ColumnName = columnName;
            this.Operand = operand;
            this.Value = value;
        }

        internal void SetSanitizedName(string colname)
        {
            this.ColumnName = colname;
        }

        public static IGNCondition FromConfig(
            string columnName,
            IGNSqlCondition operand,
            object value
            )
        {
            return new IGNCondition(columnName, operand, value);
        }

        protected virtual string ValueAsString()
        {
            var value = $"{Value}";
            if (Value.GetType() == typeof(string))
            {
                value = "'" + value + "'";
            }
            if (Value.GetType() == typeof(bool))
            {
                value = $"{((bool)Value ? "'1'" : "'0'")}";
            }
            return value;
        }

        public override string ToString()
        {
            var operation = "";
            switch (Operand)
            {
                case IGNSqlCondition.In:
                    operation = "IN";
                    break;
                case IGNSqlCondition.Between:
                    operation = "BETWEEN";
                    break;
                case IGNSqlCondition.Like:
                    operation = "LIKE";
                    break;
                case IGNSqlCondition.Eq:
                    operation = "=";
                    break;
                case IGNSqlCondition.Ge:
                    operation = ">=";
                    break;
                case IGNSqlCondition.Gt:
                    operation = ">";
                    break;
                case IGNSqlCondition.Le:
                    operation = "<=";
                    break;
                case IGNSqlCondition.Lt:
                    operation = "<";
                    break;
                case IGNSqlCondition.Ne:
                    operation = "<>";
                    break;
            }
            var value = ValueAsString();
            return $"{ColumnName} {operation} {value}";
        }
    }
}
