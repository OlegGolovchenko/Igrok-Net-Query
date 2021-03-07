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

using System;

namespace IGNQuery.Attributes
{
    /// <summary>
    /// Marker for database column.
    /// You need it to be able to use ORM parts of IGNQuery
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DatabaseColumn: Attribute
    {
        private readonly string column;
        private readonly string defValue;
        private readonly bool canHaveNull;
        private readonly bool isGenerated;
        private readonly bool primary;
        private readonly string length;

        /// <summary>
        /// Column Name
        /// </summary>
        public string Column { 
            get
            {
                return this.column;
            } 
        }

        /// <summary>
        /// Default value
        /// </summary>
        public string DefValue
        {
            get
            {
                return this.defValue;
            }
        }

        /// <summary>
        /// Is column Primary Key
        /// </summary>
        public bool IsPrimary
        {
            get
            {
                return this.primary;
            }
        }

        /// <summary>
        /// Is column generated
        /// </summary>
        public bool IsGenerated
        {
            get
            {
                return this.isGenerated;
            }
        }

        /// <summary>
        /// Can have null
        /// </summary>
        public bool CanHaveNull
        {
            get
            {
                return this.canHaveNull;
            }
        }

        /// <summary>
        /// Length of column if applicable
        /// </summary>
        public string Length
        {
            get
            {
                return length;
            }
        }

        /// <summary>
        /// Mark field as databound
        /// </summary>
        /// <param name="column">column name</param>
        /// <param name="primary">primary key</param>
        /// <param name="isGenerated">generated column</param>
        /// <param name="canHaveNull">can have null value</param>
        /// <param name="defValue">default value</param>
        /// <param name="length">length of column if applicable</param>
        public DatabaseColumn(string column, bool primary, bool isGenerated, bool canHaveNull, string defValue, string length)
        {
            this.column = column;
            this.primary = primary;
            this.isGenerated = isGenerated;
            this.canHaveNull = canHaveNull;
            this.defValue = defValue;
            this.length = length;
        }
    }
}
