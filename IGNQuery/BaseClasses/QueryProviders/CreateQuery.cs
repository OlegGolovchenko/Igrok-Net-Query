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

using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public class CreateQuery : ICreateQuery
    {
        private IGNQueriable queriable;

        public CreateQuery(string email,IDataDriver dataDriver)
        {
            this.queriable = IGNQueriable.Begin(email, dataDriver);
        }

        public IGNQueriable AsIgnQueriable()
        {
            return this.queriable;
        }

        public string GetResultingString()
        {
            return this.queriable.ToString();
        }

        public IQueryResult StoredProcedure(string name, IQueryResult content, [Optional] IEnumerable<TableField> parameters)
        {
            this.queriable.Create().StoredProcedure(name, content.AsIgnQueriable(), 
                () => parameters!=null?parameters.Select(
                        x=>Tuple.Create(
                            x.Name,
                            x.FromStringToType(),
                            x.StringLengthFromType()
                            )
                    ) : new List<Tuple<string, Type, int>>()
                ).
                IfNotExists();
            return this;
        }

        public IQueryResult TableIfNotExists(string name, IEnumerable<TableField> fields)
        {
            this.queriable.Create().Table(name,
                ()=>fields.Select(
                    x=>Tuple.Create(
                        x.Name,
                        x.FromStringToType(),
                        x.StringLengthFromType(),
                        !x.CanHaveNull,
                        x.Generated,
                        x.Primary,
                        x.DefValueFromString()
                        )
                    )
                ).IfNotExists();
            return this;
        }
    }
}
