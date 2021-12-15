using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class AddTaskViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        private UserModel user;
        private BoardModel Board;

        public AddTaskViewModel(UserModel u)
        {
            this.Controller = u.Controller;
            this.user = u;
            this.Board = u.GetBoard();
        }

        //==============================================================================================================================================================
        //Methods.

        public void Add()
        {
            try
            {
                Board.AddTask(user, DueDate, Title, Description);
                Message = "The Task has been added.";
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

        private DateTime _DueDate = DateTime.Now;
        public DateTime DueDate
        {
            get => _DueDate;
            set
            {
                _DueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        private string _Descritpion;
        public string Description
        {
            get => _Descritpion;
            set
            {
                _Descritpion = value;
                RaisePropertyChanged("Descritpion");
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
