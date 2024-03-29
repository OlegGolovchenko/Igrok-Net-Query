﻿//#############################################
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

namespace IGNQuery.Interfaces.QueryProvider
{
    /// <summary>
    /// Insert query representation
    /// </summary>
    public interface IInsert
    {
        /// <summary>
        /// Into operand
        /// </summary>
        /// <param name="table">table to insert into</param>
        /// <param name="fields">fields to insert for</param>
        /// <param name="existsCheck">check if exists</param>
        /// <returns>values query</returns>
        IValuesQuery Into(string table, IEnumerable<string>fields, bool existsCheck);

        /// <summary>
        /// Into select operand
        /// </summary>
        /// <param name="table">table to insert into</param>
        /// <param name="fields">fields to insert for</param>
        /// <param name="selectFields">fields to select</param>
        /// <param name="distinct">select distinct or not</param>
        /// <param name="existsCheck">check if exists</param>
        /// <returns>values query</returns>
        ISelect IntoSelect(string table, IEnumerable<string> fields,
            IEnumerable<string> selectFields, bool distinct, bool existsCheck);
    }
}
