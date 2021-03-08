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

using System;

namespace IGNQuery.BaseClasses.Business
{
    public delegate string GetDbTypeFunc(
        Type type, 
        int length);

    public delegate string GetDefaultValueFunc(
        bool isRequired, 
        bool isGenerated, 
        object defaultValue);

    public delegate string GetDbAutoGenFunc(
        bool generated,
        Type colType,
        int length);

    public class TableColumnConfiguration
    {
        public string ColumnName { get; private set; }

        public Type ColumnType { get; private set; }

        public int Length { get; private set; }

        public bool Required { get; private set; }

        public bool Generated { get; private set; }

        public bool Primary { get; private set; }

        public object DefValue { get; private set; }

        internal TableColumnConfiguration(
            string name,
            Type type, 
            int length, 
            bool required, 
            bool generated, 
            bool primary, 
            object defvalue
            )
        {
            ColumnName = name;
            ColumnType = type;
            Length = length;
            Required = required;
            Generated = generated;
            Primary = primary;
            DefValue = defvalue;
        }

        public static TableColumnConfiguration FromConfig(
            string name,
            Type type,
            int length,
            bool required,
            bool generated,
            bool primary,
            object defvalue
            )
        {
            return new TableColumnConfiguration(
                name,
                type,
                length,
                required,
                generated,
                primary,
                defvalue);
        }

        public static TableColumnConfiguration FromTableField(TableField tableField)
        {
            return new TableColumnConfiguration(
                tableField.Name,
                tableField.FromStringToType(),
                tableField.StringLengthFromType(),
                !tableField.CanHaveNull,
                tableField.Generated,
                tableField.Primary,
                tableField.DefValueFromString());
        }

        public string AsCreateTableQueryField(
            GetDbTypeFunc getDbType,
            GetDefaultValueFunc getDefaultValue,
            GetDbAutoGenFunc getDbAutoGen)
        {
            return $"{ColumnName} {getDbType(ColumnType, Length)} " +
                $"{(Required ? "NOT NULL" : "NULL")}" +
                $"{getDefaultValue(Required, Generated, DefValue)}" +
                $"{getDbAutoGen(Required, ColumnType, Length)}";
        }
    }
}
