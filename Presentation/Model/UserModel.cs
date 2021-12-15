using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class UserModel : NotifiableModelObject
    {
        public UserModel(BackendController controller, string email, string nickname) : base(controller)
        {
            this.Email = email;
            this.Nickname = nickname;
        }

        //===================================================================================================
        //Method.

        public BoardModel GetBoard()
        {
            return new BoardModel(Controller, this);
        }

        //===================================================================================================
        //Bindings.

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                RaisePropertyChanged("Nickname");
            }
        }
    }
}
