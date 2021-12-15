using System;
using Data = IntroSE.Kanban.Backend.DataAccessLayer.Objects;
using Controls = IntroSE.Kanban.Backend.DataAccessLayer.Controls;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class Task : PersistedObject<Data.Task>
    {
        private int Id;
        private string _emailassignee;
        private DateTime CreationTime;
        private string Title;
        private string Description;
        private DateTime DueDate;
        private int _columnid;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        // difining a task with respect to the limitations that are defined in the assignment instructions
        public Task(int id, string emailassigne, DateTime creationTime, string title, string description, DateTime dueDate,int columnid)
        {
            //that new title must have at most 50 letters and not empty.
            if (string.IsNullOrWhiteSpace(title) || title.Length > 50)
            {
                log.Error("The title must not be max 50 letters and not empty.");
                throw new Exception("The title must not be max 50 letters and not empty.");
            }
            //that new title must have at most 300 letters.
            if (description != null)
            {
                if (description.Length > 300)
                {
                    log.Error("The title must not be max 300 letters.");
                    throw new Exception("The title must not be max 300 letters.");
                }
            }
            //the duedate most be after the date of the creationtime date.
            if (DateTime.Compare(DateTime.Now, dueDate) > 0)
            {
                log.Error("Invalid duedate.");
                throw new Exception("Invalid duedate.");
            }
            this.Id = id;
            this.emailassignee = emailassigne;
            this.CreationTime = creationTime;
            this.Title = title;
            this.Description = description;
            this.DueDate = dueDate;
            this.columnid = columnid;
            Save("insert");
        }

        public Task()
        {
        }

        //For Unit Tests.
        public Task(string email, int id)
        {
            this.Id = id;
            this.emailassignee = email;
        }


        // enable to update the title of an existing task 
        // with the limitations of a valid title as defined in the assignment instructions
        public void edittitle(string email, string title)
        {
            //that new title must have at most 50 letters and not empty.
            if (string.IsNullOrWhiteSpace(title) || title.Length > 50)
            {
                log.Error("The title must not be max 50 letters and not empty.");
                throw new Exception("The title must not be max 50 letters and not empty.");
            }
            //Only the assigned user can change the title of the task.
            if (this.emailassignee != email)
            {
                log.Error("Only the assignee has the permission tp update task.");
                throw new Exception("Only the assignee has the permission tp update task.");
            }
            this.Title = title;
            log.Info("The title is updated.");
            update(id, "title", title);
        }


        // enable to update the description of an existing task 
        // with the limitations of a valid description as defined in the assignment instructions
        public void editdescription(string email, string description)
        {
            if (description != null)
            {
                //that new title must have at most 300 letters.
                if (description.Length > 300)
                {
                    log.Error("The title must not be max 300 letters.");
                    throw new Exception("The title must not be max 300 letters.");
                }
            }
            //Only the assigned user can change the title of the task.
            if (this.emailassignee != email)
            {
                log.Error("Only the assignee has the permission tp update task.");
                throw new Exception("Only the assignee has the permission tp update task.");
            }
            this.Description = description;
            log.Info("The description is updated.");
            update(id,"description",description);
        }


        // enable to update the due date of an existing task 
        // with the limitations of a valid due date as defined in the assignment instructions
        public void editduedate(string email, DateTime duedate)
        {
           //the duedate most be after the date of the creationtime date.
           if (DateTime.Compare(DateTime.Now, duedate) > 0)
           {
                log.Error("Invalid duedate.");
                throw new Exception("Invalid duedate.");
           }
           //Only the assigned user can changethe title of the task.
           if (this.emailassignee != email)
           {
                log.Error("Only the assignee has the permission tp update task.");
                throw new Exception("Only the assignee has the permission tp update task.");
           }
            this.DueDate = duedate;
            log.Info("The due date is updated.");
            update(id, "duedate", duedate);
        }

        //===================================================================================================================================
        //Implementing PersistedObject interface

        public void import(Data.Task dal)
        {
            this.Id = dal.Id;
            this.emailassignee = dal.emailassignee;
            this.CreationTime = dal.creationtime;
            this.Title = dal.title;
            this.Description = dal.description;
            this.DueDate = dal.duedate;
            this.columnid = dal.columnID;
        }

        public void Save(string action)
        {
            if (action == "insert")
            {
                Controls.TaskControl control = new Controls.TaskControl();
                control.Insert(this.todal());
            }
            else if (action == "delete")
            {
                DalControl control = new Controls.TaskControl();
                control.Delete(this.todal());
            }
        }

        public Data.Task todal()
        {
            return new Data.Task(id, emailassignee, creationtime, title, description, duedate, columnid);
        }


        //-------------------------------------------
        //Functions for updating the database.
        public void update(int id, string attributeName, string attributeValue)
        {
            DalControl control = new Controls.TaskControl();
            control.Update(id, attributeName, attributeValue);
        }

        public void update(int id, string attributeName, int attributeValue)
        {
            DalControl control = new Controls.TaskControl();
            control.Update(id, attributeName, attributeValue);
        }

        public void update(int id, string attributeName, DateTime attributeValue)
        {
            DalControl control = new Controls.TaskControl();
            control.Update(id, attributeName, attributeValue);
        }

        //===================================================================================================================================
        // getters and setters.

        public int id
        {
            get { return Id; }
        }

        public string emailassignee
        {
            get { return _emailassignee; }
            set { this._emailassignee = value; }
        }

        public DateTime creationtime
        {
            get { return CreationTime; }
        }

        public string title
        {
            get { return Title; }
        }

        public string description
        {
            get { return Description; }
        }

        public DateTime duedate
        {
            get { return DueDate; }
        }

        public int columnid
        {
            get { return _columnid; }
            set { this._columnid = value; }
        }

    }
}
