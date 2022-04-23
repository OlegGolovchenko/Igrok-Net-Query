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
    internal class Insert : ValuesQuery, IInsert
    {
        internal Insert(IGNQueriable queriable) : base(queriable)
        {

        }

        public IValuesQuery Into(string table, IEnumerable<string> fields, bool existsCheck)
        {
            queriable.AddOperation("INSERT INTO", queriable.SanitizeName(table), "");
            var sanitizedFields = fields.Select(name => queriable.SanitizeName(name));
            queriable.AddOperation("", $"({string.Join(",", sanitizedFields)}) VALUES", ""); 
            if (existsCheck)
            {
                queriable.IfExists(IGNDbObjectTypeEnum.Table, table, "");
            }
            return this;
        }

        public ISelect IntoSelect(string table, IEnumerable<string> fields,
            IEnumerable<string> selectFields, bool distinct, bool existsCheck)
        {
            queriable.AddOperation("INSERT INTO", queriable.SanitizeName(table), "");
            var sanitizedFields = fields.Select(name => queriable.SanitizeName(name));
            queriable.AddOperation("", $"({string.Join(",", sanitizedFields)}) ", "");
            string columns = selectFields == null ? "*" : string.Join(",", selectFields.Select(x => queriable.SanitizeName(x)));
            if (distinct)
            {
                operation = $"SELECT DISTINCT {columns}";
            }
            else
            {
                operation = $"SELECT {columns}";
            }
            if (existsCheck)
            {
                queriable.IfExists(IGNDbObjectTypeEnum.Table, table, "");
            }
            return this;
        }
    }
}
