using IntroSE.Kanban.Backend.DataAccessLayer.Controls;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Objects
{
    class User : DalObject
    {         
        private string _email;
        public const string Email = "email";
        private string _nickname;
        public const string Nickname = "nickname";
        private string _password;
        public const string Password = "password";
        private bool _host;
        public const string Host = "host";
        private int _boardid;
        public const string BoardId = "boardid";


        public User(int id, string email, string nickname, string password, bool host, int boardid): base(new UserControl())
        {
            this.Id = id;
            this._email = email;
            this._nickname = nickname;
            this._password = password;
            this._host = host;
            this._boardid = boardid;
        }

        public string email
        {
            get { return this._email; }
            set { this._email = value; _controller.Update(Id, Email, value); }
        }

        public string nickname
        {
            get { return this._nickname; }
            set { this._nickname = value; _controller.Update(Id, Nickname, value); }
        }

        public string password
        {
            get { return this._password; }
            set { this._password = value; _controller.Update(Id, Password, value); }
        }

        public bool host
        {
            get { return this._host; }
            set { this._host = value; _controller.Update(Id, Host, value == true ? 1 : 0); }
        }

        public int boardid
        {
            get { return this._boardid; }
            set { this._boardid = value; _controller.Update(Id, BoardId, value); }
        }
    }
}
