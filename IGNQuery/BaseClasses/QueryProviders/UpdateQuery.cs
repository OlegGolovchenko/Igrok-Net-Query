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
    public class UpdateQuery : QueryResult, IUpdateQuery
    {
        private IGNDbObjectTypeEnum objectType;
        private string name;

        public UpdateQuery(IGNQueriable queriable):base(queriable)
        {
        }

        public IConditionalQuery SetParametrizedWithCondition(string fieldName, int paramNb)
        {
            AddSetColumnCommand(fieldName, paramNb);
            return new ConditionalQuery(queriable);
        }

        public IUpdateQuery SetParametrized(string fieldName, int paramNb)
        {
            AddSetColumnCommand(fieldName, paramNb);
            return this;
        }

        private void AddSetColumnCommand(string fieldName, int paramNb)
        {
            if (this.queriable.HasSetCommand())
            {
                queriable.AddOperation("SET", $"{fieldName} = @p{paramNb}", " ");
            }
            queriable.AddOperation("", $"{fieldName} = @p{paramNb}", ", ");
        }

        public IUpdateExistenceCheckQuery Table(string table)
        {
            name = table;
            objectType = IGNDbObjectTypeEnum.Table;
            queriable.AddOperation("UPDATE TABLE", queriable.SanitizeName(table), "");
            return this;
        }

        public IUpdateQuery IfExists()
        {
            queriable.IfExists(objectType, name, "");
            return this;
        }

        public IUpdateQuery IfNotExists()
        {
            throw new NotImplementedException("IfNotExists check is not relevant for drop query");
        }
    }
}
