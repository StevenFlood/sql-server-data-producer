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

using System.Collections.ObjectModel;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.Entities.OptionEntities;
using SQLDataProducer.Entities.DatabaseEntities.Collections;
using System;
using SQLDataProducer.TaskExecuter;
using System.Windows;
using System.Windows.Threading;
using System.Threading;
using SQLDataProducer.ModalWindows;
using System.Linq;
using System.Windows.Data;
using System.ComponentModel;

namespace SQLDataProducer.Model
{
    public class ApplicationModel : INotifyPropertyChanged
    {


        public ApplicationModel()
        {
            Tables = new TableEntityCollection();
            WorkFlowManager = new WorkflowManager();

            ExecutionItems = new ExecutionItemCollection();
            SelectedExecutionItem = ExecutionItems.FirstOrDefault();

            //ExecutionItemsWithWarningsView = System.Windows.Data.CollectionViewSource.GetDefaultView(ExecutionItems);

            _executionItemsWithWarningsSource = new CollectionViewSource { Source = ExecutionItems };
            ExecutionItemsWithWarningsView = _executionItemsWithWarningsSource.View;
            ExecutionItemsWithWarningsView.Filter = new Predicate<object>( obj  =>
            {
                ExecutionItem t = obj as ExecutionItem;
                if (t != null)
                {
                    return t.HasWarning;
                }
                return true;
            });
        }

        CollectionViewSource _executionItemsWithWarningsSource;


        TableEntityCollection _tables;
        /// <summary>
        /// The list of tables available in the database
        /// </summary>
        public TableEntityCollection Tables
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

                    IsTableSelected = _selectedTable != null;
                    
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
            private set
            {
                if (_executionItems != value)
                {
                    _executionItems = value;

                    OnPropertyChanged("ExecutionItems");
                }

                if (value != null)
                {
                    _executionItems.CollectionChanged += (sender, e) =>
                    {
                        HavePendingChanges = true;
                    };
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
                    HaveExecutionItemSelected = value != null;
                    
                    _currentExecutionItem = value;
                    SelectedColumn = _currentExecutionItem.TargetTable.Columns.FirstOrDefault();
                    OnPropertyChanged("SelectedExecutionItem");
                    OnPropertyChanged("SelectedColumn");
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
                    _connectionString = SQLDataProducerSettings.Default.ConnectionString;
                }
                return _connectionString;
            }
            set
            {
                if (_connectionString != value)
                {
                    _connectionString = value;
                    SQLDataProducerSettings.Default.ConnectionString = value;
                    SQLDataProducerSettings.Default.Save();
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

        private bool _isTableSelected;
        public bool IsTableSelected
        {
            get { return _isTableSelected; }

            set
            {
                if (_isTableSelected != value)
                {
                    _isTableSelected = value;
                    OnPropertyChanged("IsTableSelected");
                }
            }
        }

        private bool _havePendingChanges;
        public bool HavePendingChanges
        {
            get { return _havePendingChanges; }

            set
            {
                if (_havePendingChanges != value)
                {
                    _havePendingChanges = value;
                    OnPropertyChanged("HavePendingChanges");
                }
            }
        }


        bool _isQueryRunning = false;
        public bool IsQueryRunning
        {
            get
            {
                return _isQueryRunning;
            }
            set
            {
                if (_isQueryRunning != value)
                {
                    _isQueryRunning = value;
                    OnPropertyChanged("IsQueryRunning");
                    OnPropertyChanged("IsExecutionIdle");
                }
            }
        }

        public bool IsExecutionIdle { get { return !IsQueryRunning; } }

        string _searchCriteria;
        public string SearchCriteria
        {
            get
            {
                return _searchCriteria;
            }
            set
            {
                if (_searchCriteria != value)
                {
                    _searchCriteria = value;
                    TablesView.Refresh();
                    SelectedTable = (TableEntity)TablesView.CurrentItem;
                    OnPropertyChanged("SearchCriteria");
                }
            }
        }

        System.ComponentModel.ICollectionView _tablesView;
        public System.ComponentModel.ICollectionView TablesView
        {
            get
            {
                return _tablesView;
            }
            set
            {
                if (_tablesView != value)
                {
                    _tablesView = value;
                    OnPropertyChanged("TablesView");
                }
            }
        }
        
        System.ComponentModel.ICollectionView _executionItemWithWarningsView;
        public System.ComponentModel.ICollectionView ExecutionItemsWithWarningsView
        {
            get
            {
                return _executionItemWithWarningsView;
            }
            set
            {
                if (_executionItemWithWarningsView != value)
                {
                    _executionItemWithWarningsView = value;
                    OnPropertyChanged("ExecutionItemsWithWarningsView");
                }
            }
        }
        

        internal void SetTablesView()
        {
            TablesView = System.Windows.Data.CollectionViewSource.GetDefaultView(Tables);

            TablesView.Filter = delegate(object obj)
            {
                TableEntity t = obj as TableEntity;

                if (String.IsNullOrEmpty(SearchCriteria))
                    return true;

                int index = t.TableName.IndexOf(
                    SearchCriteria,
                    0,
                    StringComparison.InvariantCultureIgnoreCase);

                return index > -1;
            };
        }


        string _postScriptText;
        public string PostScriptText
        {
            get
            {
                return _postScriptText;
            }
            set
            {
                if (_postScriptText != value)
                {
                    _postScriptText = value;
                    OnPropertyChanged("PostScriptText");
                }
            }
        }

        string _preScriptText;
        public string PreScriptText
        {
            get
            {
                return _preScriptText;
            }
            set
            {
                if (_preScriptText != value)
                {
                    _preScriptText = value;
                    OnPropertyChanged("PreScriptText");
                }
            }
        }

        WorkflowManager _workFlowManager;
        public WorkflowManager WorkFlowManager
        {
            get
            {
                return _workFlowManager;
            }
            set
            {
                if (_workFlowManager != value)
                {
                    _workFlowManager = value;
                    OnPropertyChanged("WorkFlowManager");
                }
            }
        }

        public void RunExecution()
        {
            if (ExecutionItems.Count == 0)
                return;

            if (string.IsNullOrEmpty(ConnectionString))
            {
                MessageBox.Show("The connection string must be set before executing");
                return;
            }

            IsQueryRunning = true;

            WorkFlowManager = new WorkflowManager();
            WorkFlowManager.RunWorkFlowAsync(Options, ConnectionString, ExecutionItems, (executionResult) =>
            {
                IsQueryRunning = false;
                // Modal window need to be started from STA thread.
                Application.Current.Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    new ThreadStart(() =>
                    {
                        // Show the Execution Summary window with results.
                        ExecutionSummaryWindow win = new ExecutionSummaryWindow(executionResult, this);
                        //win.DataContext = ;
                        win.ShowDialog();
                    })
                    );

            }
                , string.Empty
                , string.Empty);
        }


        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
