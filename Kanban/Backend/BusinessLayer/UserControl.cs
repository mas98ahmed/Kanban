using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;
using System;
using System.Linq;
using System.Collections.Generic;
using Data = IntroSE.Kanban.Backend.DataAccessLayer.Objects;
using Controls = IntroSE.Kanban.Backend.DataAccessLayer.Controls;
using IntroSE.Kanban.Backend.BusinessLayer.UserPackage.Factories;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class UserControl
    {

        private Dictionary<string, IUser> users;// hashtable of <email,User>
        private IUser activeuser=null;
        private int users_id;//the number of the users reegistered.
        private int boards_id;//the number of the boards that have benn created.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public UserControl()
        {
            users = new Dictionary<string, IUser>();
            this.users_id = 0;
            this.boards_id = 0;
        }


        //===================================================================================================================================
        //Functions connected to database.

        public void LoadData()
        {
            Controls.UserControl control = new Controls.UserControl();
            List<Data.User> newusers = control.SelectAllUsers();
            for (int i = 0; i < newusers.Count; i++)
            {
                if (newusers[i].host)
                {
                    users[newusers[i].email.ToLower()] = new HostUser();
                    this.boards_id++;
                }
                else
                {
                    users[newusers[i].email.ToLower()] = new AssigneeUser();
                }                
                users[newusers[i].email.ToLower()].import(newusers[i]);
            }
            this.users_id = newusers.Count;
        }

        public void DeleteData()
        {
            List<string> deletedusers = users.Keys.ToList<string>();
            for (int i = 0; i < deletedusers.Count; i++)
            {
                users[deletedusers[i]].Save("delete");
            }
            this.users_id = 0;
            this.boards_id = 0;
            users = new Dictionary<string, IUser>();
            activeuser = null;
        }

        //===================================================================================================================================
        
        // enable registration
        public void Register(string email, string password, string nickname, IFactory factory)
        {
            try
            {
                //checking if the user is already exists and if it's then we can't use this email.
                if (users.ContainsKey(email.ToLower()))
                {
                    log.Error("This email is already exist.");
                    throw new Exception("This email is already exist.");
                }
                users[email.ToLower()] = factory.CreateUser(users_id,nickname,password);
                users_id++;
                boards_id = factory.boardid;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // enable login
        public IUser Login(string email, string password)
        {
            try
            {
                //checking if there is an active user.
                if (this.activeuser != null)
                {
                    log.Error("There is another active user.");
                    throw new Exception("There is another active user.");
                }
                if (email == null)
                {
                    log.Error("The email is null.");
                    throw new Exception("The email is null.");
                }
                //checking if the user is already exists if it is not then there is no way to log in with no user registered.
                if (!users.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                users[email.ToLower()].Login(password);
                this.activeuser = users[email.ToLower()];
                return users[email.ToLower()];
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // enable logout
        public void Logout(string email)
        {
            try
            {
                if (email == null)
                {
                    log.Error("The email is null.");
                    throw new Exception("The email is null.");
                }
                //checking if the user is already exists if it is not then there is no way to logout with no user registered.
                if (!users.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                users[email.ToLower()].Logout();
                this.activeuser = null;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //===================================================================================================================================
        // determines whether a user is logged in
        public bool isactive(string email)
        {
            //checking if the user is already exists.
            if (!users.ContainsKey(email.ToLower()))
            {
                return false;
            }
            return users[email.ToLower()].logged_in;
        }

        //returns the id of the host if we request it to the factory in order to connect 
        //the new user to the existed board of the host 
        //and if there is no eamil host then the new user wanna create a new board.
        public int user_boardid(string hostemail)
        {
            //that meens that there is no host and we wanna create a new board.
            if (hostemail == "")
            {
                return boards_id;
            }
            //checking if there is a host with this email.
            if(!users.ContainsKey(hostemail.ToLower()))
            {
                log.Error("There is no user with this email.");
                throw new Exception("There is no user with this email.");
            }
            //checking if its a host.
            if (!users[hostemail.ToLower()].host)
            {
                log.Error("This user is not a host.");
                throw new Exception("This user is not a host.");
            }
            //this means that the new user wanna connect to an existed board.
            return users[hostemail.ToLower()].boardid;
        }
    }
}
