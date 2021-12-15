using IntroSE.Kanban.Backend.DataAccessLayer.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Objects
{
    class Board : DalObject
    {
        private string _emailhost;
        public const string EmailHost = "emailhost";


        public Board(int id, string emailhost) : base(new BoardControl())
        {
            this.Id = id;
            this._emailhost = emailhost;
        }

        public string emailhost
        {
            get { return this._emailhost; }
            set { this._emailhost = value; _controller.Update(Id, EmailHost, value); }
        }
    }
}
