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

using IGNQuery.BaseClasses.Business;
using IGNQuery.BaseClasses.ORM.Attributes;
using IGNQuery.Interfaces.ORM;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IGNQuery.BaseClasses.ORM
{
    internal class TableConfiguration
    {
        internal IList<TableColumnConfiguration> knownConfigs;
        internal IList<Tuple<string, PropertyInfo>> mappings;
        internal IList<string> columnNamesForSelect;

        internal TableConfiguration(Type entityType)
        {
            this.knownConfigs = new List<TableColumnConfiguration>();
            this.mappings = new List<Tuple<string, PropertyInfo>>();
            if(!(entityType is IEntity))
            {
                throw new InvalidOperationException("entity type must be IEntity");
            }

            var properties = entityType.GetProperties();

            foreach (var prop in properties)
            {
                string colname = string.Empty;
                bool primary = false;
                bool generated = false;
                object defaultValue = null;
                int length = 0;
                int decuimalPlaces = 0;
                bool required = false;
                bool skip = false;
                var customAttributes = prop.GetCustomAttributes(true);
                foreach (var ca in customAttributes)
                {
                    if (ca is IgnoreAttribute ignoreAtt)
                    {
                        skip = true;
                    }
                    if (ca is ColumnAttribute colNameAtt)
                    {
                        colname = colNameAtt.ColumnName;
                    }
                    if(ca is PrimaryKeyAttribute primaryKeyAtt)
                    {
                        primary = true;
                    }
                    if(ca is AutoIncrement autoIncrementAtt)
                    {
                        generated = true;
                    }
                    if(ca is DefaultValueAttribute defaultValueAtt)
                    {
                        defaultValue = defaultValueAtt.Value;
                    }
                    if(ca is LengthAttribute lengthAtt)
                    {
                        length = lengthAtt.Length;
                        decuimalPlaces = lengthAtt.DecimalPlaces;
                    }
                }
                if (skip)
                {
                    continue;
                }
                mappings.Add(Tuple.Create(colname, prop));
                var config = TableColumnConfiguration.FromConfig
                    (colname,
                     prop.PropertyType,
                     length,
                     required,
                     generated,
                     primary,
                     defaultValue,
                     decuimalPlaces);
                this.knownConfigs.Add(config);
                this.columnNamesForSelect.Add(colname);
            }
        }

        public static TableConfiguration FromEntity(Type entity)
        {
            var config = new TableConfiguration(entity);
            return config;
        }
    }
}
