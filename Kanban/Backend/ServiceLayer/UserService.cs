using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using B = IntroSE.Kanban.Backend.BusinessLayer.UserPackage;
using IntroSE.Kanban.Backend.BusinessLayer.UserPackage.Factories;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {

        private UserControl c;

        public UserService()
        {
            this.c = new UserControl();
        }


        public Response LoadData()
        {
            try
            {
                c.LoadData();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response DeleteData()
        {
            try
            {
                c.DeleteData();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
        //------------------------------------------------------------------------------------------------
        //implementing the design pattern.

        public Response Register(string email, string password, string nickname)
        {
            try
            {
                c.Register(email, password, nickname, new HostFactory(email, c.user_boardid("")));
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response Register(string email, string password, string nickname, string emailhost)
        {
            try
            {
                c.Register(email, password, nickname, new AssigneeFactory(email, c.user_boardid(emailhost)));
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        //------------------------------------------------------------------------------------------------

        public Response<User> Login(string email, string password)
        {
            try
            {
                //converting a  user(Business Layer) to a user(Service Layer).
                B.IUser user = c.Login(email, password);
                User newuser = new User(user.email, user.nickname);
                //The response.
                return new Response<User>(newuser);
            }
            catch (Exception e)
            {
                return new Response<User>(e.Message);
            }
        }

        public Response Logout(string email)
        {
            try
            {
                c.Logout(email);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        //===================================================================================================================================
        //The function that returning if the user is logged in. 
        internal bool isactive(string email)
        {
            return c.isactive(email);        
        }

        //===================================================================================================================================
    }
}
