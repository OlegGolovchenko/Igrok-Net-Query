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
using IGNQuery.Enums;
using IGNQuery.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public partial class IGNQueriable
    {

        public static IGNQueriable Begin(string email, IDataDriver dataDriver, string key)
        {
            return new IGNQueriable(dataDriver, email, key);
        }

        public static IGNQueriable FromQueryString(string query, string email, IDataDriver dataDriver, string key)
        {
            return new IGNQueriable(dataDriver, email, key)
            {
                fullQuery = query,
                exists = true,
                canExecute = true
            };
        }

        public static void SetExists(bool exists, IGNQueriable queriable)
        {
            queriable.exists = queriable.exists && exists;
        }

        public static void SetCanExecute(ExistsEnum existsFunc, IGNQueriable queriable)
        {
            switch (existsFunc)
            {
                case ExistsEnum.Exists:
                    queriable.canExecute = queriable.exists;
                    break;
                case ExistsEnum.NotExists:
                    queriable.canExecute = !queriable.exists;
                    break;
                default:
                    queriable.canExecute = true;
                    break;
            }
        }

        private string GetPrimaryKey(string tableName,
            IEnumerable<TableColumnConfiguration> fieldsInfo)
        {
            var pkFields = fieldsInfo.Where(x => x.Primary).Select(x => SanitizeName(x.ColumnName));
            return $"CONSTRAINT PK_{tableName} PRIMARY KEY({string.Join(",", pkFields)})";
        }

        private static string GetDefaultValue(bool isRequired,
            bool isGenerated, object defValue, DialectEnum dialect)
        {
            var valQuotesFormat = "('{0}')";
            var valNoQuotesFormat = "({0})";
            if (dialect == Enums.DialectEnum.MSSQL)
            {
                valQuotesFormat = "''{0}''";
            }
            var defVal = defValue?.ToString();
            if (defValue is bool boolean)
            {
                defVal = boolean ? string.Format(valNoQuotesFormat, "1") : string.Format(valNoQuotesFormat, "0");
            }
            if (defValue is string)
            {
                defVal = string.Format(valQuotesFormat, defVal);
            }
            return isRequired && !isGenerated && defValue != null ? $" DEFAULT {defVal}" : "";
        }

        private static string GetDbAutoGenFunc(bool generated,
            Type clrType, int length, IDataDriver driver)
        {
            return generated ? driver.GetDbAutoGenFor(clrType, length) : "";
        }

        private static string GetDbType(Type clrType, int length, int decPos)
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
            else if (clrType.Equals(typeof(byte[])))
            {
                return $"BINARY({length})";
            }
            else if (clrType.Equals(typeof(TimeSpan)))
            {
                return $"TIME";
            }
            return null;
        }
    }
}
