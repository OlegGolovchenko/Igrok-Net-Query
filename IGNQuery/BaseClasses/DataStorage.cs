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
using System.Collections.Generic;
using System.Linq;

namespace IGNQuery.BaseClasses
{
    public class DataStorage
    {
        private readonly IDataProvider dataProvider;
        private readonly IList<DataTable<DataEntity>> tables;
        private readonly IList<DataTable<DataEntity>> removedTables;

        public DataStorage(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            this.tables = new List<DataTable<DataEntity>>();
            this.removedTables = new List<DataTable<DataEntity>>();
        }

        public void Init()
        {
            if(dataProvider != null)
            {
                foreach(var table in tables)
                {
                    table.Init();
                }
            }
        }

        public void CommitModelChanges()
        {
            if (dataProvider != null)
            {
                foreach (var rt in removedTables)
                {
                    rt.Drop();
                }
                removedTables.Clear();
            }
        }

        public void AddTableFor<T>() where T: DataEntity
        {
            this.tables.Add(new DataTable<T>(this.dataProvider) as DataTable<DataEntity>);
        }

        public void RemoveTableFor<T>() where T: DataEntity
        {
            this.tables.Remove(new DataTable<T>(this.dataProvider) as DataTable<DataEntity>);
            this.removedTables.Add(new DataTable<T>(this.dataProvider) as DataTable<DataEntity>);
        }

        public DataTable<T> Table<T>() where T : DataEntity
        {
            if(this.tables.Any(x=>x is T))
            {
                var entity = this.tables.SingleOrDefault(x => x is T);
                if(entity != null)
                {
                    return entity as DataTable<T>;
                }
            }
            return null;
        }
    }
}
