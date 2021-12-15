using System;
using Data = IntroSE.Kanban.Backend.DataAccessLayer.Objects;
using Controls = IntroSE.Kanban.Backend.DataAccessLayer.Controls;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    class HostUser : IUser
    {
        private int _id;
        private string _email;
        private string _nickname;
        private string password;
        private bool _host = true;
        private int _boardid;
        private bool _logged_in = false;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        // difining a user with respect to the limitations that are defined in the assignment instructions
        public HostUser(int id, string email, string password, string nickname, int boardid)
        {
            //checking if the nickname is null or just spaces.
            if (string.IsNullOrWhiteSpace(nickname))
            {
                log.Error("The nickname is null.");
                throw new Exception("The nickname is null.");
            }
            //checks if the nickname is empty.
            if (nickname.Length < 1)
            {
                log.Error("The nickname is empty.");
                throw new Exception("The nickname is empty.");
            }
            //checking if the email is valid.
            if (!IsValidEmail(email))
            {
                log.Error("The email is invalid.");
                throw new Exception("The email is invalid.");
            }
            //checking if the password is compatable for the restrictions.
            if (!validPass(password))
            {
                log.Error("Invalid password.");
                throw new Exception("Invalid password.");
            }
            this.id = id;
            this._email = email;
            this._nickname = nickname;
            this.password = password;
            this._boardid = boardid;
            log.Info("The registering succeeded.");
            //adding the new user to the database.
            Save("insert");
        }

        public HostUser()
        {
        }

        /// enabling logging in to the system 
        /// with respect to the limitations that are defined in the assignment instructions
        public void Login(string password)
        {
            if (password == null)
            {
                log.Error("The password is null.");
                throw new Exception("The password is null.");
            }
            //checking if the user is already connected.
            if (this.logged_in)
            {
                log.Error("The user is already active.");
                throw new Exception("The user is already active.");
            }
            //checking if the password is right.
            if (password != this.password)
            {
                log.Error("The password is wrong.");
                throw new Exception("The password is wrong.");
            }
            this.logged_in = true;
            log.Info("The user is active.");
        }

        // enabling logging out to the system for an active user only 
        public void Logout()
        {
            //checking if the user is offline then we can't do a logout while he hasn't logged in yet.
            if (!this.logged_in)
            {
                log.Error("This user wasn't active.");
                throw new Exception("This user wasn't active.");
            }
            this.logged_in = false;
            log.Info("The user logged out");
        }

        //===================================================================================================================================
        //Database methods.

        public Data.User todal()
        {
            return new Data.User(id, email, nickname, password, true, boardid);
        }

        public void import(Data.User dal)
        {
            this.id = dal.Id;
            this._email = dal.email;
            this._nickname = dal.nickname;
            this.password = dal.password;
            this._host = dal.host;
            this.boardid = dal.boardid;
        }

        public void Save(string action)
        {
            if (action == "insert")
            {
                Controls.UserControl control = new Controls.UserControl();
                control.Insert(this.todal());
            }
            else if (action == "delete")
            {
                DalControl control = new Controls.UserControl();
                control.Delete(this.todal());
            }
        }

        //===================================================================================================================================
        //getters and setters.

        public bool logged_in
        {
            get { return this._logged_in; }
            set { this._logged_in = value; }
        }

        public int id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        public string email
        {
            get { return this._email; }
            set { this._email = value; }
        }

        public string nickname
        {
            get { return this._nickname; }
            set { this._nickname = value; }
        }

        public int boardid
        {
            get { return this._boardid; }
            set { this._boardid = value; }
        }

        public bool host
        {
            get { return this._host; }
            set { this._host = value; }
        }

        //===================================================================================================================================

        //checkig the validity of an email
        // with respect to the limitations that are defined in the assignment instructions
        private bool IsValidEmail(string email)
        {
            //checking if its null or if there is any space.
            if (string.IsNullOrWhiteSpace(email)) { return false; }
            //checking if it contains @.
            if (!email.Contains("@"))
            {
                return false;
            }
            //checking if it contains . .
            if (!email.Contains("."))
            {
                return false;
            }

            if (!System.Char.IsLetter(email[0]))
            {
                return false;
            }
            //checking if there is two symbols in a row.
            for (int i = 0; i < email.Length - 1; i++)
            {
                char a = email[i];
                char b = email[i + 1];
                if ((!System.Char.IsUpper(a) && !System.Char.IsLower(a) && !System.Char.IsNumber(a)) && (!System.Char.IsUpper(b) && !System.Char.IsLower(b) && !System.Char.IsNumber(b)))
                {
                    return false;
                }
            }
            //checking if everything after the point contains letters.
            string[] subs = email.Split('.');
            for (int i = 0; i < subs[subs.Length - 1].Length; i++)
            {
                char c = subs[subs.Length - 1][i];
                if (!System.Char.IsLetter(c))
                {
                    return false;
                }
            }
            //checking if the text before the point is not empty.
            string[] sub = email.Split('@');
            if (sub[0] == "")
            {
                return false;
            }

            for (int i = 0; i < subs[subs.Length - 1].Length; i++)
            {
                char c = subs[subs.Length - 1][i];
                if (!System.Char.IsLetter(c) && c != '.')
                {
                    return false;
                }
            }
            return true;
        }

        // checkig the validity of a password
        // with respect to the limitations that are defined in the assignment instructions
        private bool validPass(string password)
        {
            bool upper = false, lower = false, num = false, flag = true;
            foreach (char a in password)
            { 
                if (System.Char.IsUpper(a))
                    upper = true;
                if (System.Char.IsLower(a))
                    lower = true;
                if (System.Char.IsNumber(a))
                    num = true;
            }
            //checking if the password has at least one capital letter and small one also has a number.
            if (!(upper && lower && num))
                flag = false;
            //checking if the password's length is at least 5 letters and max 25.
            if (password.Length < 5 || password.Length > 25)
                flag = false;
            //checks if the password is null or just spaces(empty).
            if (string.IsNullOrWhiteSpace(password))
                flag = false;
            return flag;
        }

        //===================================================================================================================================
    }
}
