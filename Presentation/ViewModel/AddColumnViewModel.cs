using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class AddColumnViewModel : NotifiableObject
    {
        private UserModel user;
        private BoardModel Board;
        public BackendController Controller { get; private set; }

        //=============================================================================================================================================================================

        public AddColumnViewModel(UserModel u)
        {
            this.Controller = u.Controller;
            this.user = u;
            this.Board = u.GetBoard();
        }

        //=============================================================================================================================================================================
        //Methods.

        public UserModel Back()
        {
            return this.user;
        }

        public void Add()
        {
            try
            {
                Board.AddColumn(user ,Name, Ordinal);
                Message = "The Column has been added.";
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        //=============================================================================================================================================================================
        //Bindings.

        private string _Name;
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                RaisePropertyChanged("Name");
            }
        }

        private int _Ordinal;
        public int Ordinal
        {
            get => _Ordinal;
            set
            {
                _Ordinal = value;
                RaisePropertyChanged("Ordinal");
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

