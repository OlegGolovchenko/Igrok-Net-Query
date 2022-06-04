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

using IGNQuery.Interfaces.ORM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IGNQuery.BaseClasses.ORM
{
    internal class Table<T> : ITable<T> where T : IEntity
    {
        private Database database;
        private TableConfiguration configuration;

        internal Table(Database database)
        {
            this.database = database;
            this.configuration = this.database.knownConfigs.SingleOrDefault(cfg => cfg.Item1 == typeof(T)).Item2;
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public T Find()
        {
            throw new NotImplementedException();
        }

        public IList<T> FindAll()
        {
            throw new NotImplementedException();
        }

        public void SaveOrUpdate(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
