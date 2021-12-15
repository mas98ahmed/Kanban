using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class UpdateTaskViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        private UserModel user;
        private TaskModel selectedTask;
        private BoardModel Board;

        public UpdateTaskViewModel(UserModel u, TaskModel selectedTask)
        {
            this.Controller = u.Controller;
            this.user = u;
            this.selectedTask = selectedTask;
            this.Board = u.GetBoard();
            this.Title = selectedTask.Title;
            this.CreationTime = selectedTask.CreationTime;
            this.DueDate = selectedTask.DueDate;
            this.Description = selectedTask.Descritpion;
        }

        //==============================================================================================================================================================
        //Methods.

        public void Update()
        {
            try
            {
                Controller.Update(selectedTask, DueDate, Title, Description);
                Board.Update(selectedTask, DueDate, Title, Description);
                Message = "The Task has been updated.";
            }
            catch(Exception e)
            {
                Message = e.Message;
            }
        }

        public UserModel Back()
        {
            return user;
        }

        //==============================================================================================================================================================
        //Bindings.

        private string _Title;
        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _Description;
        public string Description
        {
            get => _Description;
            set
            {
                _Description = value;
                RaisePropertyChanged("Descritpion");
            }
        }

        private string _emailassignee;
        public string EmailAssignee
        {
            get => _emailassignee;
            set
            {
                _emailassignee = value;
                RaisePropertyChanged("EmailAssignee");
            }
        }

        private DateTime _DueDate;
        public DateTime DueDate
        {
            get => _DueDate;
            set
            {
                _DueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        private DateTime _CreationTime;
        public DateTime CreationTime
        {
            get => _CreationTime;
            set
            {
                _CreationTime = value;
                RaisePropertyChanged("CreationTime");
            }
        }

        //Respond for the method.
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged("Message");
            }
        }
    }
}
