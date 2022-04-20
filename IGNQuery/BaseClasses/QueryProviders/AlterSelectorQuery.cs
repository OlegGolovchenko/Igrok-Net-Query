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

using IGNQuery.BaseClasses.Business;
using IGNQuery.Enums;
using IGNQuery.Interfaces.QueryProvider;
using System;
using System.Reflection;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public class AlterSelectorQuery<T, U> : IAlterSelectorQuery<T, U>
        where T : IQuery
        where U : IExistanceCheck<T>
    {
        private string name;
        private IGNQueriable queriable;
        private bool isAddQuery;
        private string delimiter;

        public AlterSelectorQuery(IGNQueriable queriable, bool isAddQuery, string delimiter)
        {
            this.queriable = queriable;
            this.isAddQuery = isAddQuery;
            this.delimiter = delimiter;
        }

        public virtual U Column(TableColumnConfiguration column)
        {
            name = column.ColumnName;
            string operand = isAddQuery ? "" : "COLUMN";
            queriable.AddOperation(operand, queriable.FormatFieldOptionals(column), "");
            Type u = typeof(U);

            ConstructorInfo ci = u.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null, new Type[] { typeof(IGNQueriable), typeof(string), typeof(IGNDbObjectTypeEnum), typeof(string) }, null);
            return (U)ci.Invoke(new object[] { queriable, name, IGNDbObjectTypeEnum.Database, delimiter });
        }
    }
}
