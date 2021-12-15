using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer.Controls;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Objects
{
    class Column : DalObject
    {
        private string _name;
        public const string Name = "name";
        private int _limit;
        public const string Limit = "limit";
        private int _ordinal;
        public const string Ordinal = "ordinal";
        public int _boardid;
        public const string BoardId = "boardid";


        public Column(int id, string name, int limit, int ordinal, int boardid) : base(new ColumnControl())
        {
            this.Id = id;
            this._name = name;
            this._limit = limit;
            this._ordinal = ordinal;
            this._boardid = boardid;
        }
        
        public string name
        {
            get { return _name; }
            set { this._name = value; _controller.Update(Id, Name, value); }
        }
        
        public int ordinal
        {
            get { return _ordinal; }
            set { this._ordinal = value; _controller.Update(Id, Ordinal, value); }
        }
        
        public int limit
        {
            get { return _limit; }
            set { this._limit = value; _controller.Update(Id, Limit, value); }
        }

        public int boardid
        {
            get { return _boardid; }
            set { this._boardid = value; _controller.Update(Id, BoardId, value); }
        }
    }
}
