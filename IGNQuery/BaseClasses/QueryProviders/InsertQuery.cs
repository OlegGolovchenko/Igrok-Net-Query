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

using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;
using System.Collections.Generic;
using System.Linq;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public class InsertQuery : IInsertQuery
    {

        private IGNQueriable queriable;
        private bool firstRow;

        public InsertQuery(string email, IDataDriver dataDriver)
        {
            queriable = IGNQueriable.Begin(email, dataDriver);
            this.firstRow = true;
        }

        public IInsertQuery AddRow(IEnumerable<FieldValue> values)
        {
            if (this.firstRow)
            {
                this.queriable.Values(values.Select(x => x.ObjectValue));
            }
            else
            {
                this.queriable.AddRow(values.Select(x => x.ObjectValue));
            }
            return this;
        }

        public IInsertQuery AddRowWithParams(IEnumerable<int> paramNumbers)
        {
            if (this.firstRow)
            {
                this.queriable.ValuesWithParams(paramNumbers);
            }
            else
            {
                this.queriable.AddRowWithParams(paramNumbers);
            }
            return this;
        }

        public IGNQueriable AsIgnQueriable()
        {
            return this.queriable;
        }

        public string GetResultingString()
        {
            return this.queriable.ToString();
        }

        public IInsertQuery Into(string table, IEnumerable<string> fields)
        {
            this.queriable.Insert().Into(table, fields);
            return this;
        }

        public IInsertQuery Next()
        {
            this.firstRow = false;
            return this;
        }

        public IInsertQuery Values()
        {
            this.firstRow = true;
            return this;
        }
    }
}
