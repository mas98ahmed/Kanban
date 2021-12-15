using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class BoardViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        public UserModel user { get; private set; }
        public BoardModel Board { get; private set; }

        //=========================================================================================================================================================

        public BoardViewModel(UserModel user)
        {
            this.Controller = user.Controller;
            this.user = user;
            this.Board = user.GetBoard();
            Host = user.Email == Board.HostEmail ? Visibility.Visible : Visibility.Collapsed;
        }

        //=========================================================================================================================================================
        //Task Methods

        public void DeleteTask()
        {
            try
            {
                Board.DeleteTask(user,SelectedTask);
                SelectedTask = null;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void AdvanceTask()
        {
            try
            {
                Board.AdvanceTask(user,SelectedTask);
                SelectedTask = null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //=========================================================================================================================================================
        //Column's methods.

        public void RemoveColumn()
        {
            try
            {
                Board.RemoveColumn(SelectedColumn.ordinal);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void MoveColumnRight()
        {
            try
            {
                Board.MoveColumnRight(SelectedColumn.ordinal);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void MoveColumnLeft()
        {
            try
            {
                Board.MoveColumnLeft(SelectedColumn.ordinal);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //===========================================================================================================================================================
        //Bindings.

        //Column's stuff

        private ColumnModel _selectedColumn;
        public ColumnModel SelectedColumn
        {
            get
            {
                return _selectedColumn;
            }
            set
            {
                _selectedColumn = value;
                RaisePropertyChanged("SelectedColumn");
            }
        }

        //----------------------------------------------------------------

        //Task's stuff

        private bool _enableForwardTask = false;

        public bool EnableForwardTask
        {
            get => _enableForwardTask;
            private set
            {
                _enableForwardTask = value;
                RaisePropertyChanged("EnableForwardTask");
            }
        }

        public void SortTasks()
        {
            Board.SortTasks();
        }

        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                _selectedTask = value;
                EnableForwardTask = value != null;
                RaisePropertyChanged("SelectedTask");
            }
        }

        private Visibility _host;
        public Visibility Host
        {
            get => _host;
            set
            {
                _host = value;
                RaisePropertyChanged("Host");
            }
        }

        //===========================================================================================================================================================
        //Filtering.

        public void ResetFilter()
        {
            Board.ResetFilter();
            Searching_Word = "";
        }

        public void Filter()
        {
            Board.Filter(Searching_Word);
        }

        //The searching text.
        private string _search;
        public string Searching_Word
        {
            get => _search;
            set
            {
                _search = value;
                RaisePropertyChanged("Searching_Word");
            }
        }

        //---------------------------------------------------------------------------------------------
        //is host?.

        public bool IsAssigned()
        {
            return user.Email == SelectedTask.EmailAssignee;
        }

        public bool IsHost()
        {
            return user.Email == Board.HostEmail;
        }
    }
}
