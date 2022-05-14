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

using IGNActivation.Client.Interfaces;
using IGNQuery.BaseClasses.Business;
using IGNQuery.BaseClasses.QueryProviders;
using IGNQuery.Enums;
using System;
using System.Collections.Generic;
using System.Data;

namespace IGNQuery.Interfaces
{
    public interface IDataDriver:IDisposable
    {
        DialectEnum Dialect { get; }

        void AssignActivator(IActivationClient activator, string email);

        void AssignActivator(string email, string key=null);

        void Execute(IGNQueriable query);

        void ExecuteWithParameters(IGNQueriable query, IEnumerable<IGNParameterValue> args);

        void ExecuteStoredProcedure(string procName, IEnumerable<IGNParameterValue> args);

        DataTable ReadData(IGNQueriable query);

        DataTable ReadDataWithParameters(IGNQueriable query, IEnumerable<IGNParameterValue> args);

        DataTable ReadDataFromStoredProcedure(string procName, IEnumerable<IGNParameterValue> args);

        void IfTableNotExists(string name, IGNQueriable queriable);

        void IfDatabaseNotExists(string name, IGNQueriable queriable);

        void IfStoredProcedureNotExists(string name, IGNQueriable queriable);

        void IfViewNotExists(string name, IGNQueriable queriable);

        void IfIndexNotExists(string name, string table, IGNQueriable queriable);

        void IfColumnNotExists(string name, string table, IGNQueriable queriable);

        void IfTableExists(string name, IGNQueriable queriable);

        void IfDatabaseExists(string name, IGNQueriable queriable);

        void IfStoredProcedureExists(string name, IGNQueriable queriable);

        void IfViewExists(string name, IGNQueriable queriable);

        void IfIndexExists(string name, string table, IGNQueriable queriable);

        void IfColumnExists(string name, string table, IGNQueriable queriable);

        string GoTerminator();

        string GetDbAutoGenFor(Type clrType, int length);

        string GetDatabaseName(string dbFunction);
    }
}
