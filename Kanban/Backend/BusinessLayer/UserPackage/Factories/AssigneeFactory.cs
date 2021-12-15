using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage.Factories
{
    class AssigneeFactory : IFactory
    {
        private string email;
        private int _boardid;

        public AssigneeFactory(string email, int boardid)
        {
            this.email = email;
            this.boardid = boardid;
        }
        public IUser CreateUser(int id, string nickname, string password)
        {
            int idboard = boardid;
            boardid++;
            return new AssigneeUser(id,email,password,nickname,idboard);
        }

        //=====================================================================
        public int boardid
        {
            get { return this._boardid; }
            set { this._boardid = value; }
        }
    }
}
