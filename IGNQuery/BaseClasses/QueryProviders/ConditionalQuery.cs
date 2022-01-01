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
using IGNQuery.Interfaces.QueryProvider;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public class ConditionalQuery : QueryResult, IConditionalQuery
    {
        internal ConditionalQuery(IGNQueriable queriable):base(queriable)
        {
        }

        public IConditionalQuery Where(IGNConditionWithParameter condition)
        {
            condition.SetSanitizedName(queriable.SanitizeName(condition.ColumnName));
            queriable.AddOperation("WHERE", $"{condition}", " ");
            return this;
        }

        public IConditionalQuery Condition(IGNConditionWithParameter condition)
        {
            condition.SetSanitizedName(queriable.SanitizeName(condition.ColumnName));
            queriable.AddOperation("", $"{condition}", " ");
            return this;
        }

        public IConditionalQuery And()
        {
            queriable.AddOperation("ADD", "", " ");
            return this;
        }

        public IConditionalQuery Or()
        {
            queriable.AddOperation("OR", "", " ");
            return this;
        }

        public IConditionalQuery Not()
        {
            queriable.AddOperation("NOT", "", " ");
            return this;
        }
    }
}
