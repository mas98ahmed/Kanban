using System;
using Data = IntroSE.Kanban.Backend.DataAccessLayer.Objects;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    interface IUser : PersistedObject<Data.User>
    {
        string email { get; }
        string nickname { get; set; }
        bool logged_in { get; }
        int boardid { get; }
        bool host { get; }

        void Login(string password);

        void Logout();
    }
}
