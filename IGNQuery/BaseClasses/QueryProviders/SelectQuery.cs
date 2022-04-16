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
using IGNQuery.Interfaces.QueryProvider;
using System.Collections.Generic;
using System.Linq;

namespace IGNQuery.BaseClasses.QueryProviders
{
    internal class SelectQuery : QueryResult,
        ISelectQuery, 
        ISelectExistenceCheckQuery, 
        ISelecteableQuery
    {
        private IGNDbObjectTypeEnum objectType;
        private string name;
        private readonly bool distinct;
        private readonly IEnumerable<string> fieldNames;

        internal SelectQuery(IGNQueriable queriable, bool distinct, IEnumerable<string> fieldNames = null) : base(queriable)
        {
            this.distinct = distinct;
            this.fieldNames = fieldNames;
        }

        public IExistanceCheck<IConditionalQuery> From(string table)
        {
            objectType = IGNDbObjectTypeEnum.Table;
            name = table;
            string columns = fieldNames == null ? "*" : string.Join(",", fieldNames.Select(x => queriable.SanitizeName(x)));
            string operand = distinct ? "SELECT DISTINCT" : "SELECT";
            queriable.AddOperation(operand, columns, "");
            queriable.AddOperation("FROM", queriable.SanitizeName(table), " ");
            return new ExistanceCheck<IConditionalQuery>(queriable, name, objectType);
        }

        public ISelecteableQuery IfExists()
        {
            queriable.IfExists(objectType, name, "");
            if (fieldNames != null)
            {
                foreach (string colname in fieldNames)
                {
                    queriable.IfExists(IGNDbObjectTypeEnum.Column, colname, name);
                }
            }
            return this;
        }

        public ISelectQuery Join(string sourceTableName, string destTableName, string sourceKeyName, string destKeyName, bool inner = true, bool left = false, bool right = false)
        {
            string operand = "JOIN";
            if (inner)
            {
                operand = "INNER " + operand;
            }
            if (left)
            {
                operand = "LEFT " + operand;
            }
            if (right)
            {
                operand = "RIGHT " + operand;
            }
            queriable.AddOperation(operand, queriable.SanitizeName(destTableName), " ");
            string parameter = $"{queriable.SanitizeName(sourceTableName)}.{queriable.SanitizeName(sourceKeyName)} = " +
                $"{queriable.SanitizeName(destTableName)}.{queriable.SanitizeName(destKeyName)}";
            queriable.AddOperation("ON", parameter, " ");
            return this;
        }

        public IConditionalQuery WithCondition()
        {
            return new ConditionalQuery(queriable);
        }
    }
}
