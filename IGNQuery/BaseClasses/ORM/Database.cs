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

using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Interfaces;
using IGNQuery.Interfaces.ORM;
using System;
using System.Collections.Generic;

namespace IGNQuery.BaseClasses.ORM
{
    public class Database : IDatabase
    {
        internal IList<Tuple<Type,TableConfiguration>> knownConfigs;
        internal IDataDriver dbDriver;
        internal string email;
        internal string key;
        public Database(string email, IDataDriver dbDriver, string key)
        {
            this.knownConfigs = new List<Tuple<Type,TableConfiguration>>();
            this.dbDriver = dbDriver;
            this.email = email;
            this.key = key;
        }

        public void CreateTable<T>() where T : IEntity
        {
            var config = TableConfiguration.FromEntity(typeof(T));
            this.knownConfigs.Add(Tuple.Create(typeof(T), config));
            IGNQueriable.Begin(email, dbDriver, key).
                         Create().
                         Table(nameof(T).ToLower(), true, config.knownConfigs);
        }

        public ITable<T> Table<T>() where T : IEntity
        {
            return new Table<T>(this);
        }
    }
}
