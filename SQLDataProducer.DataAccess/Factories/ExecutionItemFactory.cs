﻿// Copyright 2012 Peter Henell

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.DatabaseEntities;
using System.Xml;

namespace SQLDataProducer.DataAccess.Factories
{
    // TODO: Refactor into static factory class
    public class ExecutionItemFactory
    {
        TableEntityDataAccess _tda;
        public ExecutionItemFactory(string connectionString)
        {
            _tda  = new TableEntityDataAccess(connectionString);
        }

        public ExecutionItemFactory(TableEntityDataAccess _tda)
        {
            this._tda = _tda;
        }

        public IEnumerable<ExecutionItem> GetExecutionItemsFromTables(IEnumerable<TableEntity> tables)
        {
            // Clone the selected table so that each generation of that table is configurable uniquely
            foreach (var table in tables)
            {
                yield return CloneFromTable(table);
            }
        }

        public ExecutionItem CloneFromTable(TableEntity table)
        {
            TableEntity clonedTable = _tda.CloneTable(table);
            return new ExecutionItem(clonedTable);
        }

        public void Save(ExecutionItemCollection execItems, string fileName)
        {
            using (XmlWriter xmlWriter = XmlTextWriter.Create(fileName))
            {
                execItems.WriteXml(xmlWriter);
            }
        }

        public ExecutionItemCollection Load(string fileName)
        {
            // The logic here is that we only load(and save) relevant configuration.
            // When we load, we will only load up 
            //      The execution items and their tables.
            //      The columns and their selected generator and it's parameters  
            //      
            // We then get the correct information from the database and applies the loaded changes to the tables and columns retrieved from the database.
            // By doing this we will get all the columns correctly from the database, and we only need to store the configured parameter values in the savefile.
            ExecutionItemCollection loadedExecCollection = new ExecutionItemCollection();

            ExecutionItemCollection completeExecCollection = new ExecutionItemCollection();
            using (XmlReader reader = XmlReader.Create(fileName))
            {
                loadedExecCollection.ReadXml(reader);
            }

            
            foreach (var item in loadedExecCollection)
            {
                TableEntity table = _tda.GetTableAndColumns(item.TargetTable.TableSchema, item.TargetTable.TableName);
                foreach (var newColumn in table.Columns)
                {
                    foreach (var c2 in item.TargetTable.Columns)
                    {
                        if (newColumn.ColumnName == c2.ColumnName)
                        {
                            newColumn.Generator = newColumn.PossibleGenerators.Where(gen => gen.GeneratorName == c2.Generator.GeneratorName).Single();
                            // If this column is foreign key generator then use the foreign keys we just read from the DB
                            if (newColumn.Generator.GeneratorName.ToLower().Contains("foreign"))
                                continue;

                            newColumn.Generator.SetGeneratorParameters(c2.Generator.GeneratorParameters);
                        }
                    }
                }
                ExecutionItem ei = new ExecutionItem(table, item.Description);
                completeExecCollection.Add(ei);
            }

            return completeExecCollection;
        }
    }
}
