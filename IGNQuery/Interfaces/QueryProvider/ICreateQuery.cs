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

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IGNQuery.Interfaces.QueryProvider
{
    public interface ICreateQuery : IQueryResult
    {
        IQueryResult TableIfNotExists(string name, IEnumerable<TableField> fields);

        IQueryResult TableIfNotExists(string name, IEnumerable<string> filedDefinitions);

        /// <summary>
        /// Creates stored procedure You should drop stored procedure if it exists before doing this
        /// </summary>
        /// <param name="name">Stored procedure name</param>
        /// <param name="content">body of stored procedure</param>
        /// <param name="parameters">parameters of stored procedure</param>
        /// <returns>QueryResult</returns>
        IQueryResult StoredProcedure(string name, IQueryResult content, [Optional] IEnumerable<TableField> parameters);
    }
}
