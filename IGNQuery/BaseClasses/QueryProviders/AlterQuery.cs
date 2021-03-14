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
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.QueryProvider;
using System;
using System.Collections.Generic;

namespace IGNQuery.BaseClasses.QueryProviders
{
    public class AlterQuery : IAlterQuery
    {
        private readonly IGNQueriable queriable;
        private IGNQueriable subquery;
        private readonly IList<string> preppedSubqueries;
        private readonly string email;
        private readonly IDataDriver dataDriver;

        public AlterQuery(string email, IDataDriver dataDriver)
        {
            this.preppedSubqueries = new List<string>();
            this.queriable = IGNQueriable.Begin(email, dataDriver);
            this.subquery = IGNQueriable.Begin(email, dataDriver);
            this.email = email;
            this.dataDriver = dataDriver;
        }

        public IAlterQuery Add()
        {
            this.subquery.Add();
            return this;
        }

        public IAlterQuery Alter()
        {
            this.subquery.Alter();
            return this;
        }

        public IGNQueriable AsIgnQueriable()
        {
            return this.queriable;
        }

        public IAlterQuery Column(TableField column)
        {
            if (this.subquery.IsDropColumn)
            {
                this.subquery.Column(column.Name);
            }
            else
            {
                this.subquery.Column(column.Name,
                    column.FromStringToType(),
                    column.StringLengthFromType(),
                    !column.CanHaveNull,
                    column.Generated,
                    column.DefValueFromString());
            }
            return this;
        }

        public IAlterQuery Drop()
        {
            this.subquery.Drop();
            return this;
        }

        public string GetResultingString()
        {
            return this.queriable.ToString();
        }

        public IAlterQuery Go()
        {

            var query = this.subquery.ToString();
            if (this.subquery.IsAddColumn)
            {
                if (this.dataDriver.Dialect == DialectEnum.MSSQL &&
                    this.preppedSubqueries.Count > 0)
                {
                    query = query.Substring(4);
                }
            }
            else if (this.subquery.IsDropColumn)
            {
                if (this.dataDriver.Dialect == DialectEnum.MSSQL &&
                   this.preppedSubqueries.Count > 0)
                {
                    query = query.Substring(11);
                }
            }
            this.preppedSubqueries.Add(query);
            var subQuery = string.Join(",",this.preppedSubqueries);
            this.subquery = IGNQueriable.FromQueryString(subQuery, this.email, this.dataDriver);
            this.queriable.ReplaceAlterSubquery(this.subquery);
            return this;
        }

        public IAlterQuery Next()
        {
            if (this.subquery.IsAddColumn)
            {
                var query = this.subquery.ToString();
                if (this.dataDriver.Dialect == DialectEnum.MSSQL &&
                   this.preppedSubqueries.Count > 0)
                {
                    query = query.Substring(4);
                }
                this.preppedSubqueries.Add(query);
                this.subquery = IGNQueriable.Begin(this.email, this.dataDriver);
            }
            else if (this.subquery.IsDropColumn)
            {
                var query = this.subquery.ToString();
                if (this.dataDriver.Dialect == DialectEnum.MSSQL &&
                   this.preppedSubqueries.Count > 0)
                {
                    query = query.Substring(11);
                }
                this.preppedSubqueries.Add(query);
                this.subquery = IGNQueriable.Begin(this.email, this.dataDriver);
            }
            else 
            { 
                    throw new Exception("Only Add column can be called for multiple columns");                
            }
            return this;
        }

        public IAlterQuery TableIfExists(string tableName)
        {
            if (this.dataDriver.Dialect == DialectEnum.MSSQL)
            {
                this.queriable.Alter().Table(tableName, this.subquery).IfExists();
            }
            else if(this.dataDriver.Dialect == DialectEnum.MySQL)
            {
                this.queriable.Alter().Table(tableName, this.subquery);
            }
            return this;
        }
    }
}
