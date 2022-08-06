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
using System.Collections.Generic;

namespace IGNQuery.Interfaces.QueryProvider
{
    /// <summary>
    /// Alter query representation
    /// </summary>
    public interface IAlter
    {
        /// <summary>
        /// Alter first column
        /// </summary>
        /// <param name="column">new column configuration</param>
        /// <param name="existsCheck">check if exists</param>
        /// <returns>next alter column query</returns>
        IAlterColumn AlterColumn(TableColumnConfiguration column, bool existsCheck);
        /// <summary>
        /// Add first column
        /// </summary>
        /// <param name="column">new column configuration</param>
        /// <param name="existsCheck">check if not exists</param>
        /// <returns>next add column query</returns>
        IAddColumn AddColumn(TableColumnConfiguration column, bool existsCheck);
        /// <summary>
        /// Drop first column
        /// </summary>
        /// <param name="column">column to drop</param>
        /// <param name="existsCheck">check if exists</param>
        /// <returns>next drop column query</returns>
        IDropColumn DropColumn(string column, bool existsCheck);
        /// <summary>
        /// Adds primary key constraint
        /// </summary>
        /// <param name="name">name of constraint without PK_</param>
        /// <param name="columns">columns to use as key</param>
        /// <param name="existsCheck">check if columns and constraint exist</param>
        /// <returns>next alter query</returns>
        IAlter AddPrimaryKey(string name, IList<string> columns, bool existsCheck);
        /// <summary>
        /// Drop primary key constraint
        /// </summary>
        /// <param name="name">name of constraint to drop without PK_</param>
        /// <param name="existsCheck">check if constraint exists</param>
        /// <returns>next alter query</returns>
        IAlter DropPrimaryKey(string name, bool existsCheck);
        /// <summary>
        /// Add default constraint
        /// </summary>
        /// <param name="column">name of column to add default</param>
        /// <param name="name">name of constraint without DF_</param>
        /// <param name="index">index of parameter for default value</param>
        /// <param name="existsCheck">check if constraint exists</param>
        /// <returns>query result</returns>
        IQueryResult AddDefault(string column, string name, int index, bool existsCheck);
        /// <summary>
        /// Drop default constraint
        /// </summary>
        /// <param name="column">column to drop default from</param>
        /// <param name="existsCheck">check if constraint exists</param>
        /// <returns>query result</returns>
        IQueryResult DropDefault(string column, bool existsCheck);
        /// <summary>
        /// Adds foreign key constraint
        /// </summary>
        /// <param name="name">constraint name</param>
        /// <param name="sourceColumn">source column</param>
        /// <param name="targetTable">joined table</param>
        /// <param name="column">column of joined table</param>
        /// <param name="existsCheck">check if exsists</param>
        /// <returns>query result</returns>
        IQueryResult AddForeignKey(string name,string sourceColumn, string targetTable, string column, bool existsCheck);
        /// <summary>
        /// Drops foreign key constraint
        /// </summary>
        /// <param name="name">key name</param>
        /// <param name="checkExists">exists check</param>
        /// <returns>quiery result</returns>
        IQueryResult DropForeignKey(string name, bool checkExists);
    }
}
