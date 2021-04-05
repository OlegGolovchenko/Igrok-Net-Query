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

using IGNQuery.BaseClasses.Business;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public partial class IGNQueriable
    {
        private string GetParams(Func<IEnumerable<IGNParameter>> fields)
        {
            var parameterList = fields?.Invoke();
            return string.Join(",", parameterList.Select(x => x.AsQuery(GetDbType)));
        }

        private IEnumerable<string> CompileFieldsInfo(string tableName,
            IEnumerable<TableColumnConfiguration> fieldsInfo)
        {
            var fieldDescs = fieldsInfo.Select(x => x.AsCreateTableQueryField(
                this.dataDriver.Dialect, GetDbType, GetDefaultValue, GetDbAutoGenFunc)).ToList();
            fieldDescs.Add(GetPrimaryKey(tableName, fieldsInfo));
            return fieldDescs;
        }

        private string FormatFieldOptionals(Type colType, 
            int length, bool required, bool generated, object defValue)
        {
            return $"{(required ? "NOT NULL" : "NULL")}" +
                   $"{GetDefaultValue(required, generated, defValue)}" +
                   $"{GetDbAutoGenFunc(generated, colType, length)}";
        }

        private string GetPrimaryKey(string tableName, IEnumerable<TableColumnConfiguration> fieldsInfo)
        {
            var pkFields = fieldsInfo.Where(x => x.Primary).Select(x => SanitizeName(x.ColumnName));
            return $"CONSTRAINT PK_{tableName} PRIMARY KEY({string.Join(",", pkFields)})";
        }

        private string GetDefaultValue(bool isRequired,
            bool isGenerated, object defValue)
        {
            var valQuotesFormat = "('{0}')";
            var valNoQuotesFormat = "({0})";
            if(this.dataDriver.Dialect == Enums.DialectEnum.MSSQL)
            {
                valQuotesFormat = "''{0}''";
            }
            var defVal = defValue?.ToString();
            if (defValue is bool boolean)
            {
                defVal = boolean ? string.Format(valQuotesFormat,"1") : string.Format(valQuotesFormat, "0");
                if(this.dataDriver.Dialect == Enums.DialectEnum.MySQL)
                {
                    defVal = boolean ? string.Format(valNoQuotesFormat, "1") : string.Format(valNoQuotesFormat, "0");
                }
            }
            if (defValue is string)
            {
                defVal = string.Format(valQuotesFormat, defVal);
            }
            return isRequired && !isGenerated && defValue != null ? $" DEFAULT {defVal}" : "";
        }

        private string GetDbAutoGenFunc(bool generated, Type clrType, int length)
        {
            return generated ? this.dataDriver.GetDbAutoGenFor(clrType, length) : "";
        }

        private string GetDbType(Type clrType, int length, int decPos)
        {
            if (clrType.Equals(typeof(long)))
            {
                return "BIGINT";
            }
            else if (clrType.Equals(typeof(bool)))
            {
                return "BIT";
            }
            else if (clrType.Equals(typeof(string)))
            {
                return $"NVARCHAR({length})";
            }
            else if (clrType.Equals(typeof(DateTime)))
            {
                return "DATETIME";
            }
            else if (clrType.Equals(typeof(int)))
            {
                return "INT";
            }
            else if (clrType.Equals(typeof(short)))
            {
                return "SMALLINT";
            }
            else if (clrType.Equals(typeof(byte)))
            {
                return "TINYINT";
            }
            else if (clrType.Equals(typeof(decimal)))
            {
                return $"DECIMAL({length},{decPos})";
            }
            else if (clrType.Equals(typeof(double)) || clrType.Equals(typeof(float)))
            {
                return "FLOAT";
            }
            else if(clrType.Equals(typeof(byte[])))
            {
                return $"BINARY({length})";
            }
            else if (clrType.Equals(typeof(TimeSpan)))
            {
                return $"TIME";
            }
            return null;
        }
        private object FormatSuffix(string suffix)
        {
            return string.IsNullOrWhiteSpace(suffix) ? suffix : "\n" + suffix;
        }

        private string FormatPrefix(string prefix)
        {
            return string.IsNullOrWhiteSpace(prefix) ? prefix : prefix + "\n";
        }

        private void BuildDropQuery(string objType)
        {
            this.query = string.Format(
                this.format,
                FormatPrefix(this.prefix),
                objType,
                this.afterObjType,
                this.objectName,
                FormatSuffix(this.suffix),
                this.dataDriver.GoTerminator()
            );
        }

        private void BuildCreateQuery(string objType)
        {
            this.query = string.Format(
                this.format,
                FormatPrefix(this.prefix),
                objType,
                this.afterObjType,
                this.objectName,
                this.querySpecificPart,
                FormatSuffix(this.suffix),
                this.dataDriver.GoTerminator());
        }

        private void BuildAlterQuery(string objType)
        {
            this.query = string.Format(
                this.format,
                FormatPrefix(this.prefix),
                objType,
                this.afterObjType,
                this.objectName,
                this.subquery,
                FormatSuffix(this.suffix),
                this.dataDriver.GoTerminator());
        }

        private void BuildUseQuery()
        {
            this.query = string.Format(
                this.format,
                this.objectName,
                this.dataDriver.GoTerminator()
            );
        }

        private void BuildAddQuery()
        {
            this.query = string.Format(
                this.format,
                "",
                this.objectName,
                this.querySpecificPart
            );
        }

        private void BuildAlterColumnQuery(string objType)
        {
            this.query = string.Format(
                this.format,
                objType,
                this.objectName,
                this.querySpecificPart
            );
        }

        private void BuildDropColumnQuery(string objType)
        {
            this.query = string.Format(
                this.format,
                objType,
                this.objectName,
                this.querySpecificPart
            );
        }

        private void BuildUpdateQuery()
        {
            this.query = string.Format(
                this.format,
                this.objectName,
                this.querySpecificPart,
                this.conditional);
        }

        private void BuildInsertQuery()
        {
            this.query = string.Format(
                this.format,
                this.querySpecificPart,
                this.insertValuesQueryPart);
        }

        private void BuildSelectQuery()
        {
            string fields;
            if (fieldNames.Count() == 0)
            {
                fields = "*";
            }
            else
            {
                fields = string.Join(",", this.fieldNames);
            }
            this.query = string.Format(
                this.format,
                fields,
                this.objectName,
                this.querySpecificPart,
                this.conditional);
        }

        private void BuildDeleteQuery()
        {
            this.query = string.Format(
                this.format,
                this.objectName,
                this.conditional);
        }
    }
}
