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

using IGNQuery.Interfaces.QueryProvider;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IGNQuery.Interfaces;
using IGNQuery.Exceptions;
using System.Data.Common;
using System;
#if !NETFULL
using System.Reflection.Metadata;
#endif
using IGNQuery.Attributes;

namespace IGNQuery.BaseClasses
{
    public class DataEntity
    {

        internal string TableName
        {
            get
            {
                DatabaseTable tableAttribute = null;
#if NETFULL
                tableAttribute = GetType().GetCustomAttributes(true).Where(ca => ca is DatabaseTable).Cast<DatabaseTable>().SingleOrDefault();
#else
                tableAttribute = (DatabaseTable)GetType().GetTypeInfo().GetCustomAttribute(typeof(DatabaseTable));
#endif
                return tableAttribute?.TableName;
            }
        }

        private IEnumerable<FieldValue> FieldValues
        {
            get
            {
                var result = new List<FieldValue>();
                var fields = GetType().GetFields();
                foreach (var field in fields)
                {
                    var fieldName = "";
                    var fieldType = "";
                    var attribs = field.GetCustomAttributes(true);
                    if (attribs.Any(ca => ca is DatabaseColumn))
                    {
                        var attrib = attribs.Where(ca => ca is DatabaseColumn).Cast<DatabaseColumn>().FirstOrDefault();
                        if (attrib != null)
                        {
                            fieldName = attrib.FullColumn.Split(' ')[0];
                            fieldType = attrib.FullColumn.Split(' ')[1];
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(fieldName))
                    {
                        if (fieldType == TableField.TYPE_LONG)
                        {
                            result.Add(new FieldValue
                            {
                                LongValue = (long)field.GetValue(this)
                            });
                        }
                        else if (fieldType == TableField.TYPE_BOOLEAN)
                        {
                            result.Add(new FieldValue
                            {
                                BooleanValue = (bool)field.GetValue(this)
                            });
                        }
                        else if (fieldType == TableField.TYPE_DATE)
                        {
                            result.Add(new FieldValue
                            {
                                DateValue = (DateTime)field.GetValue(this)
                            });
                        }
                        else if (fieldType.Contains("nvarchar"))
                        {
                            result.Add(new FieldValue
                            {
                                StringValue = (string)field.GetValue(this)
                            });
                        }
                    }
                }
                return result;
            }
        }

        private IEnumerable<TableField> TableFields
        {
            get
            {
                var result = new List<TableField>();
                var fields = GetType().GetFields();
                foreach (var field in fields)
                {

                    var attribs = field.GetCustomAttributes(true);
                    if (attribs.Any(ca => ca is DatabaseColumn))
                    {
                        var attrib = attribs.Where(ca => ca is DatabaseColumn).Cast<DatabaseColumn>().FirstOrDefault();
                        if (attrib != null)
                        {
                            result.Add(TableField.FromDatabaseColumnAttribute(attrib));
                        }
                    }
                }
                return result;
            }
        }

        internal IQueryResult GetInsertQuery(IDataProvider dataProvider)
        {
            var tableName = "";
            IEnumerable<string> columnNames = null;

            DatabaseTable tableAttribute = null;

            IQueryResult result = null;
#if NETFULL 
            tableAttribute = GetType().GetCustomAttributes(true).Where(ca => ca is DatabaseTable).Cast<DatabaseTable>().SingleOrDefault();
            columnNames = GetType().GetCustomAttributes(true).Where(ca => ca is DatabaseColumn).Cast<DatabaseColumn>().Select(dbc => dbc.FullColumn).ToList();
#else
            tableAttribute = (DatabaseTable)GetType().GetTypeInfo().GetCustomAttribute(typeof(DatabaseTable));
            columnNames = GetType().GetTypeInfo().GetCustomAttributes(true).Where(ca => ca is DatabaseColumn).Cast<DatabaseColumn>().Select(dbc => dbc.FullColumn).ToList();
#endif
            if (tableAttribute != null)
            {
                tableName = tableAttribute.TableName;
            }
            else
            {
                throw new MissingDataException("Please annotate class with DatabaseTable attribute");
            }

            if (columnNames.Count() <= 0)
            {
                throw new MissingDataException("Please annotate at least propertiy with DatabaseColumn attribute");
            }

            if (dataProvider != null)
            {
                result = dataProvider.Query().Insert().Into(tableName, columnNames).Values().AddRow(FieldValues);
            }
            return result;
        }

        internal IQueryResult GetInitTableQuery(IDataProvider dataProvider)
        {
            var tableName = "";
            DatabaseTable tableAttribute = null;

            IQueryResult result = null;
#if NETFULL 
            tableAttribute = GetType().GetCustomAttributes(true).Where(ca => ca is DatabaseTable).Cast<DatabaseTable>().SingleOrDefault();
#else
            tableAttribute = (DatabaseTable)GetType().GetTypeInfo().GetCustomAttribute(typeof(DatabaseTable));
#endif
            if (tableAttribute != null)
            {
                tableName = tableAttribute.TableName;
            }
            else
            {
                throw new MissingDataException("Please annotate class with DatabaseTable attribute");
            }

            if (TableFields.Count() <= 0)
            {
                throw new MissingDataException("Please annotate at least propertiy with DatabaseColumn attribute");
            }

            if(dataProvider != null)
            {
                result = dataProvider.Query().Create().TableIfNotExists(tableName, TableFields);
            }
            return result;
        }

        internal IQueryResult GetDropQuery(IDataProvider dataProvider)
        {
            var tableName = "";
            DatabaseTable tableAttribute = null;
            IQueryResult result = null;
#if NETFULL
            tableAttribute = GetType().GetCustomAttributes(true).Where(ca => ca is DatabaseTable).Cast<DatabaseTable>().SingleOrDefault();
#else
            tableAttribute = (DatabaseTable)GetType().GetTypeInfo().GetCustomAttribute(typeof(DatabaseTable));
#endif
            if (tableAttribute != null)
            {
                tableName = tableAttribute.TableName;
            }
            else
            {
                throw new MissingDataException("Please annotate class with DatabaseTable attribute");
            }

            if (dataProvider != null)
            {
                result = dataProvider.Query().Drop().TableIfExists(tableName);
            }
            return result;
        }

        internal void ReadFromDataReader(DbDataReader dataReader)
        {
            if (dataReader.HasRows)
            {
                var fields = GetType().GetFields();
                foreach(var field in fields)
                {
                    var fieldName = "";
                    var fieldType = "";
                    var attribs = field.GetCustomAttributes(true);
                    if (attribs.Any(ca=>ca is DatabaseColumn))
                    {
                        var attrib = attribs.Where(ca => ca is DatabaseColumn).Cast<DatabaseColumn>().FirstOrDefault();
                        if(attrib != null)
                        {
                            fieldName = attrib.FullColumn.Split(' ')[0];
                            fieldType = attrib.FullColumn.Split(' ')[1];
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(fieldName))
                    {
                        if(fieldType == TableField.TYPE_LONG)
                        {
                            field.SetValue(this, dataReader.GetInt64(dataReader.GetOrdinal(fieldName)));
                        }
                        else if(fieldType == TableField.TYPE_BOOLEAN)
                        {
                            field.SetValue(this, dataReader.GetBoolean(dataReader.GetOrdinal(fieldName)));
                        }
                        else if(fieldType == TableField.TYPE_DATE)
                        {
                            field.SetValue(this, dataReader.GetDateTime(dataReader.GetOrdinal(fieldName)));
                        }
                        else if (fieldType.Contains("nvarchar"))
                        {
                            field.SetValue(this, dataReader.GetString(dataReader.GetOrdinal(fieldName)));
                        }
                    }
                }
            }
        }
    }
}
