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
    /// Target for select/delete query
    /// </summary>
    public interface ITarget
    {
        /// <summary>
        /// From operand
        /// </summary>
        /// <param name="table">table to select/delete from</param>
        /// <param name="checkExists">check if exists</param>
        /// <returns>query result</returns>
        IQueryResult From(string table, bool checkExists);

        /// <summary>
        /// From operand with join
        /// </summary>
        /// <param name="table">table to select/delete from</param>
        /// <param name="checkExists">check if exists</param>
        /// <returns>joinable query</returns>
        IJoinable JoinableFrom(string table, bool checkExists);

        /// <summary>
        /// From operand with condition
        /// </summary>
        /// <param name="table">table to select/delete from</param>
        /// <param name="checkExists">check if exists</param>
        /// <returns>conditional query</returns>
        IConditional ConditionalFrom(string table, bool checkExists);
    }
}
