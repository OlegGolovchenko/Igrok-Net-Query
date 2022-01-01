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
        private string FormatFieldOptionals(Type colType, 
            int length, bool required, bool generated, object defValue,
            DialectEnum dialect)
        {
            return $"{(required ? "NOT NULL" : "NULL")}" +
                   $"{GetDefaultValue(required, generated, defValue, dialect)}" +
                   $"{GetDbAutoGenFunc(generated, colType, length, this.dataDriver)}";
        }

        internal string SanitizeName(string objName)
        {
            var sanitizedFormat = "`{0}`";
            var replace = "`.`";
            if (dataDriver.Dialect == DialectEnum.MSSQL)
            {
                sanitizedFormat = "[{0}]";
                replace = "].[";
            }
            return string.Format(sanitizedFormat, objName.Replace(".", replace));
        }

        internal IEnumerable<string> CompileFieldsInfo(string tableName,
            IEnumerable<TableColumnConfiguration> fieldsInfo)
        {
            var fieldDescs = fieldsInfo.Select(x => x.AsCreateTableQueryField(
                dataDriver, GetDbType, GetDefaultValue, GetDbAutoGenFunc)).ToList();
            fieldDescs.Add(GetPrimaryKey(tableName, fieldsInfo));
            return fieldDescs;
        }

        internal string GetParams(Func<IEnumerable<IGNParameter>> fields)
        {
            var parameterList = fields?.Invoke();
            return string.Join(",", parameterList.Select(x => x.AsQuery(GetDbType)));
        }

        internal string FormatFieldOptionals(TableField column, int decimals)
        {
            string fieldFormat = FormatFieldOptionals(column.FromStringToType(),
                    column.StringLengthFromType(),
                    !column.CanHaveNull,
                    column.Generated,
                    column.DefValueFromString(),
                    dataDriver.Dialect);
            return $"{SanitizeName(column.Name)} {GetDbType(column.FromStringToType(), column.StringLengthFromType(), decimals)} {fieldFormat}";
        }
    }
}
