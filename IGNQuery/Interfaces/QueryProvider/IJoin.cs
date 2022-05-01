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
    /// Join operand
    /// </summary>
    public interface IJoin
    {
        /// <summary>
        /// On operand
        /// </summary>
        /// <param name="sourceColumn">source table column</param>
        /// <param name="joinedColumn">target table column</param>
        /// <param name="checkExists">check if exists</param>
        /// <returns>conditional query</returns>
        IConditional On(string sourceColumn, string joinedColumn, bool checkExists);

        /// <summary>
        /// On operand for multijoin
        /// </summary>
        /// <param name="sourceColumn">source table column</param>
        /// <param name="joinedColumn">target table column</param>
        /// <param name="checkExists">check if exists</param>
        /// <returns>next joinable query</returns>
        IJoinable MultiJoinOn(string sourceColumn, string joinedColumn, bool checkExists);
    }
}
