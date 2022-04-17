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

using IGNQuery.Enums;
using IGNQuery.Interfaces.QueryProvider;
using System;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public class Join : Conditional, IJoin, IJoinable
    {
        internal string source;
        internal string destination;
        internal Join(IGNQueriable queriable) : base(queriable)
        {
        }

        public virtual IJoin InnerJoin(string joinedTable, bool checkExists)
        {
            throw new NotImplementedException("Inheriting class must override it");
        }

        public virtual IJoin LeftJoin(string joinedTable, bool checkExists)
        {
            throw new NotImplementedException("Inheriting class must override it");
        }

        public IJoinable MultiJoinOn(string sourceColumn, string joinedColumn, bool checkExists)
        {
            string parameter = $"{queriable.SanitizeName(source)}.{queriable.SanitizeName(sourceColumn)} = " +
                   $"{queriable.SanitizeName(destination)}.{queriable.SanitizeName(joinedColumn)}";
            queriable.IfExists(IGNDbObjectTypeEnum.Column, sourceColumn, source);
            queriable.IfExists(IGNDbObjectTypeEnum.Column, joinedColumn, destination);
            queriable.AddOperation("ON", parameter, " ");
            return this;
        }

        public IConditional On(string sourceColumn, string joinedColumn, bool checkExists)
        {
            string parameter = $"{queriable.SanitizeName(source)}.{queriable.SanitizeName(sourceColumn)} = " +
                   $"{queriable.SanitizeName(destination)}.{queriable.SanitizeName(joinedColumn)}";
            queriable.IfExists(IGNDbObjectTypeEnum.Column, sourceColumn, source);
            queriable.IfExists(IGNDbObjectTypeEnum.Column, joinedColumn, destination);
            queriable.AddOperation("ON", parameter, " ");
            return this;
        }

        public virtual IJoin RightJoin(string joinedTable, bool checkExists)
        {
            throw new NotImplementedException("Inheriting class must override it");
        }
    }
}
