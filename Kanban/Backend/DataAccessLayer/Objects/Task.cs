using System;
using IntroSE.Kanban.Backend.DataAccessLayer.Controls;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Objects
{
    class Task : DalObject
    {
        private string _emailassignee;
        public const string EmailAssignee = "emailassignee";
        private DateTime _creationTime;
        public const string CreationTime = "creationtime";
        private string _title;
        public const string Title = "title";
        private string _description;
        public const string Description = "description";
        private DateTime _dueDate;
        public const string DueDate = "duedate";
        private int _columnID;
        public const string ColumnID = "columnid";


        public Task(int id, string emailassignee, DateTime creationTime, string title, string description, DateTime dueDate, int columnID):base(new TaskControl())
        {
            this.Id = id;
            this._emailassignee = emailassignee;
            this._creationTime = creationTime;
            this._title = title;
            this._description = description;
            this._dueDate = dueDate;
            this._columnID = columnID;
        }
        
        public string emailassignee
        {
            get { return _emailassignee; }
            set { this._emailassignee = value; _controller.Update(Id, EmailAssignee, value); }
        }
        
        public DateTime creationtime
        {
            get { return _creationTime; }
            set { this._creationTime = value; _controller.Update(Id, CreationTime, value); }
        }
        
        public string title
        {
            get { return _title; }
            set { this._title = value; _controller.Update(Id, Title, value); }
        }
        
        public string description
        {
            get { return _description; }
            set { this._description = value; _controller.Update(Id, Description, value); }
        }
        
        public DateTime duedate
        {
            get { return _dueDate; }
            set { this._dueDate = value; _controller.Update(Id, DueDate, value); }
        }
  
        public int columnID
        {
            get { return _columnID; }
            set { this._columnID = value; _controller.Update(Id, ColumnID, value); }
        }
    }
}
