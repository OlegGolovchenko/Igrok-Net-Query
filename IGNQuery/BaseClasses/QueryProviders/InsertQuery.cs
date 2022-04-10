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
using System;
using System.Collections.Generic;
using System.Linq;

namespace IGNQuery.BaseClasses.QueryProviders
{
    internal class InsertQuery : QueryResult, IInsertQuery
    {
        private IGNDbObjectTypeEnum objectType;
        private string name;
        private readonly IList<string> values;

        public InsertQuery(IGNQueriable queriable):base(queriable)
        {
            values = new List<string>();
        }

        public IInsertQuery AddRowWithParams(IEnumerable<int> valuesRow)
        {
            if (values.Any())
            {
                values.Add($"({string.Join(",", valuesRow.Select(x => $"@p{x}"))})");
                queriable.AddOperation("", $"({string.Join(",", valuesRow.Select(x => $"@p{x}"))})", ", ");
            }
            else
            {
                throw new InvalidOperationException("Call ValuesWithParams to add first row");
            }
            return this;
        }

        public IInsertExistenceCheckQuery Into(string table, IEnumerable<string> fields)
        {
            name = table;
            objectType = IGNDbObjectTypeEnum.Table;
            queriable.AddOperation("INSERT INTO", queriable.SanitizeName(table), "");
            var sanitizedFields = fields.Select(name => queriable.SanitizeName(name));
            queriable.AddOperation("", $"({string.Join(",", sanitizedFields)})VALUES", " ");
            return this;
        }

        public IInsertQuery ValuesWithParams(IEnumerable<int> valuesRow)
        {
            if (values.Any())
            {
                throw new InvalidOperationException("Call AddRowWithParams to add another row");
            }
            values.Add($"({string.Join(",", valuesRow.Select(x => $"@p{x}"))})");
            queriable.AddOperation("", $"({string.Join(",", valuesRow.Select(x => $"@p{x}"))})", " ");
            return this;
        }

        public IInsertQuery IfExists()
        {
            queriable.IfExists(objectType, name, "");
            return this;
        }
    }
}
