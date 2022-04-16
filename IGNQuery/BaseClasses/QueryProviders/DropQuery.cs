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
using IGNQuery.Interfaces.QueryProvider;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IGNQuery.BaseClasses.QueryProviders
{
    internal class DropQuery : Selector<IExistsCheckQuery, IQueryResult>, IDropQuery
    {
        public DropQuery(IGNQueriable queriable) : base(queriable, "DROP")
        {
        }

        public override IExistsCheckQuery Index(string name, string tableName, IEnumerable<string> columns, bool unique)
        {
            throw new InvalidOperationException("This signature is for Create query only");
        }

        public override IExistsCheckQuery StoredProcedure(string name, IGNQueriable content, [Optional] IEnumerable<IGNParameter> parameters)
        {
            throw new InvalidOperationException("This signature is for Create query only");
        }

        public override IExistsCheckQuery Table(string name, IEnumerable<TableColumnConfiguration> fields)
        {
            throw new InvalidOperationException("This signature is for Create query only");
        }

        public override IExistsCheckQuery View(string name, IGNQueriable content)
        {
            throw new InvalidOperationException("This signature is for Create query only");
        }
    }
}
