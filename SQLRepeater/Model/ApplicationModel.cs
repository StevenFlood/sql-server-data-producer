﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SQLRepeater.DatabaseEntities.Entities;
using SQLRepeater.Entities.ExecutionOrderEntities;
using SQLRepeater.DataAccess;
using SQLRepeater.Entities.OptionEntities;

namespace SQLRepeater.Model
{
    public class ApplicationModel : Entities.EntityBase
    {
        ObservableCollection<TableEntity> _tables;
        /// <summary>
        /// The list of tables available in the database
        /// </summary>
        public ObservableCollection<TableEntity> Tables
        {
            get
            {
                return _tables;
            }
            set
            {
                if (_tables != value)
                {
                    _tables = value;
                    OnPropertyChanged("Tables");
                }
            }
        }

        TableEntity _selectedTable;
        /// <summary>
        /// The selected table, do not confuse with selected ExecutionItem. This is the raw database table that is selected.
        /// </summary>
        public TableEntity SelectedTable
        {
            get
            {
                return _selectedTable;
            }
            set
            {
                if (_selectedTable != value)
                {
                    _selectedTable = value;
                    OnPropertyChanged("SelectedTable");
                }
            }
        }


        bool _haveExecutionItemSelected = false;
        public bool HaveExecutionItemSelected
        {
            get
            {
                return _haveExecutionItemSelected;
            }
            set
            {
                if (_haveExecutionItemSelected != value)
                {
                    _haveExecutionItemSelected = value;
                    OnPropertyChanged("HaveExecutionItemSelected");
                }
            }
        }

        ExecutionItemCollection _executionItems;
        /// <summary>
        /// The list of ExecutionItems created. This is the list of "tables" that have been choosen to get data generated.
        /// </summary>
        public ExecutionItemCollection ExecutionItems
        {
            get
            {
                return _executionItems;
            }
            set
            {
                if (_executionItems != value)
                {
                    _executionItems = value;
                    OnPropertyChanged("ExecutionItems");
                }
            }
        }

        ExecutionItem _currentExecutionItem;
        /// <summary>
        /// The selected ExecutionItem. This an item that will get data generated. Do not confuse with SelectedTable.
        /// </summary>
        public ExecutionItem SelectedExecutionItem
        {
            get
            {
                return _currentExecutionItem;
            }
            set
            {
                if (_currentExecutionItem != value)
                {
                    if (value != null)
                    {
                        HaveExecutionItemSelected = true;
                    }
                    _currentExecutionItem = value;
                    OnPropertyChanged("SelectedExecutionItem");
                }
            }
        }


        ColumnEntity _selectedColumn;
        /// <summary>
        /// The selected Column
        /// </summary>
        public ColumnEntity SelectedColumn
        {
            get
            {
                return _selectedColumn;
            }
            set
            {
                if (_selectedColumn != value)
                {
                    _selectedColumn = value;
                    OnPropertyChanged("SelectedColumn");
                }
            }
        }

        string _connectionString = string.Empty;
        /// <summary>
        /// The configured connection string. This is generated by the ConnectionStringCreatorGUI.
        /// The value is autically stored in settings if changed.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = SQLRepeaterSettings.Default.ConnectionString;
                }
                return _connectionString;
            }
            set
            {
                if (_connectionString != value)
                {
                    _connectionString = value;
                    SQLRepeaterSettings.Default.ConnectionString = value;
                    SQLRepeaterSettings.Default.Save();
                    OnPropertyChanged("ConnectionString");
                }
            }
        }

        ExecutionTaskOptions _options;
        public ExecutionTaskOptions Options
        {
            get
            {
                return _options;
            }
            set
            {
                if (_options != value)
                {
                    _options = value;
                    OnPropertyChanged("Options");
                }
            }
        }
    }
}
