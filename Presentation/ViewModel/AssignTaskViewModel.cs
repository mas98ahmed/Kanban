using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class AssignTaskViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        private UserModel user;
        private TaskModel selectedTask;
        private BoardModel Board;

        public AssignTaskViewModel(UserModel u, TaskModel selectedTask)
        {
            this.Controller = u.Controller;
            this.user = u;
            this.selectedTask = selectedTask;
            this.Board = u.GetBoard();

        }

        //==============================================================================================================================================================
        //Methods.

        public void Assign()
        {
            try
            {
                Board.AssignTask(EmailAssignee, selectedTask);
                Message = "The Task has been Assigned.";
            }
            catch (Exception e)
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

        //Responding fo the method.
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
