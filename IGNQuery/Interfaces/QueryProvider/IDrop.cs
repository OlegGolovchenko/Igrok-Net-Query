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

namespace IGNQuery.Interfaces.QueryProvider
{
    /// <summary>
    /// Drop query representation
    /// </summary>
    public interface IDrop : IQueryResult
    {
        /// <summary>
        /// Drop database
        /// </summary>
        /// <param name="name">dbName</param>
        /// <param name="existsCheck">check if exists</param>
        /// <returns>query result</returns>
        IQueryResult Database(string name, bool existsCheck);

        /// <summary>
        /// Drop table
        /// </summary>
        /// <param name="name">table name</param>
        /// <param name="existsCheck">check if exists</param>
        /// <returns>query result</returns>
        IDrop Table(string name, bool existsCheck);

        /// <summary>
        /// Drop index
        /// </summary>
        /// <param name="name">index name</param>
        /// <param name="table">table name</param>
        /// <param name="existsCheck">check if exists</param>
        /// <returns>query result</returns>
        IDrop Index(string name, string table, bool existsCheck);

        /// <summary>
        /// Drop view
        /// </summary>
        /// <param name="name">view name</param>
        /// <param name="existsCheck">check if exists</param>
        /// <returns>query result</returns>
        IDrop View(string name,bool existsCheck);

        /// <summary>
        /// Drop stored procedure
        /// </summary>
        /// <param name="name">stored procedure name</param>
        /// <param name="existsCheck">check if exists</param>
        /// <returns>query result</returns>
        IDrop StoredProcedure(string name, bool existsCheck);
    }
}
