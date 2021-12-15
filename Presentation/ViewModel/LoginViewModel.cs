using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Presentation.Model;

namespace Presentation.ViewModel
{
    class LoginViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        public LoginViewModel()
        {
            this.Controller = new BackendController();
        }

        //==================================================================================

        public UserModel Login()
        {
            try
            {
                return Controller.Login(Email, Password);
            }
            catch(Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

        public void Register()
        {
            Message = "";
            try
            {
                if (string.IsNullOrEmpty(HostEmail))
                {
                    Controller.Register(Email, Password, Nickname);
                }
                else
                {
                    Controller.Register(Email, Password, Nickname, HostEmail);
                }
                Message = "Registered successfully";
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        //===================================================================================
        //Binding.

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

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
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

        private string _emailhost;
        public string HostEmail
        {
            get => _emailhost;
            set
            {
                _emailhost = value;
                RaisePropertyChanged("HostEmail");
            }
        }

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
