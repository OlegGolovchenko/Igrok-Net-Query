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
using IGNQuery.BaseClasses.QueryProviders;
using System.Collections.Generic;

namespace IGNQuery.Interfaces.QueryProvider
{
    /// <summary>
    /// Create query representation
    /// </summary>
    public interface ICreate
    {
        /// <summary>
        /// Create database
        /// </summary>
        /// <param name="name">dbName</param>
        /// <param name="existsCheck">check if exists</param>
        /// <returns>query result</returns>
        IQueryResult Database(string name, bool existsCheck);

        /// <summary>
        /// Create table
        /// </summary>
        /// <param name="name">table name</param>
        /// <param name="existsCheck">check if exists</param>
        /// <param name="fields">fields to add to table</param>
        /// <returns>query result</returns>
        IQueryResult Table(string name, bool existsCheck, IEnumerable<TableColumnConfiguration> fields);

        /// <summary>
        /// Create index
        /// </summary>
        /// <param name="name">index name</param>
        /// <param name="existsCheck">check if exists</param>
        /// <param name="table">table name</param>
        /// <param name="columns">columns to index on</param>
        /// <param name="unique">unique index or not</param>
        /// <returns>query result</returns>
        IQueryResult Index(string name, string table, bool unique, bool existsCheck, IEnumerable<string> columns);

        /// <summary>
        /// Create view
        /// </summary>
        /// <param name="name">view name</param>
        /// <param name="existsCheck">check if exists</param>
        /// <param name="content">content of view</param>
        /// <returns>query result</returns>
        IQueryResult View(string name, bool existsCheck, IGNQueriable content);

        /// <summary>
        /// Create stored procedure
        /// </summary>
        /// <param name="name">stored procedure name</param>
        /// <param name="checkExists">check if exists</param>
        /// <param name="content">content of stored procedure</param>
        /// <param name="parameters">parameters for stored procedure</param>
        /// <returns>query result</returns>
        IQueryResult StoredProcedure(string name, bool checkExists, IGNQueriable content, IEnumerable<IGNParameter> parameters);
    }
}
