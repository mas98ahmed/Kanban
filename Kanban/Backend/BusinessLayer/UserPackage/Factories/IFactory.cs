using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage.Factories
{
    interface IFactory
    {
        //The email is passed through the constructor.
        IUser CreateUser(int id, string nickname, string password);

        int boardid { get; }
    }
}
