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
using System;
using System.Collections.Generic;

namespace IGNQuery.BaseClasses
{
    public class DataTable<T> where T : DataEntity
    {
        private readonly IDataProvider dataProvider;
        private readonly string tableName;

        internal DataTable(IDataProvider dataProvider)
        {
            this.tableName = Activator.CreateInstance<T>().TableName;
            this.dataProvider = dataProvider;
        }

        internal void Init()
        {
            if (dataProvider != null)
            {
                dataProvider.ExecuteNonQuery(Activator.CreateInstance<T>().GetInitTableQuery(dataProvider));
            }
        }

        internal void Drop()
        {
            if (dataProvider != null)
            {
                dataProvider.ExecuteNonQuery(Activator.CreateInstance<T>().GetDropQuery(dataProvider));
            }
        }

        public List<T> ListAll()
        {
            var query = this.dataProvider.Query().Select().AllFrom(this.tableName);
            var reader = this.dataProvider.ExecuteReader(query);
            var result = new List<T>();
            if (reader.HasRows)
            {
                DataEntity row = (DataEntity)Activator.CreateInstance(typeof(T),true);
                row.ReadFromDataReader(reader);
            }
            return result;
        }

        public void Add(T entity)
        {
            if (dataProvider != null)
            {
                dataProvider.ExecuteNonQuery(entity.GetInsertQuery(dataProvider));
            }
        }
    }
}
