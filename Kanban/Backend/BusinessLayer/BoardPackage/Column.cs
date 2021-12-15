using System;
using System.Collections.Generic;
using Data = IntroSE.Kanban.Backend.DataAccessLayer.Objects;
using Controls = IntroSE.Kanban.Backend.DataAccessLayer.Controls;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]
namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class Column : PersistedObject<Data.Column>
    {
        private int _id;
        private string _name;
        private List<Task> _tasks;
        private int _limit;
        private int _ordinal;
        private int boardid;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Column(int id, string name, int limit, int ordinal, int boardid)
        {
            //checking if the name iss not empty and its max length is 15.
            if(string.IsNullOrWhiteSpace(name) || name.Length > 15)
            {
                log.Error("The column's name must not be max 15 letters and not empty.");
                throw new Exception("The column's name must not be max 15 letters and not empty.");
            }
            this.id = id;
            this.name = name;
            this.limit = limit;
            this.ordinal = ordinal;
            this.boardid = boardid;
            this._tasks = new List<Task>();
            Save("insert");
        }

        //For Unit Tests
        public Column(int id, string name, int limit, int ordinal)
        {
            //checking if the name iss not empty and its max length is 15.
            if (string.IsNullOrWhiteSpace(name) || name.Length > 15)
            {
                log.Error("The column's name must not be max 15 letters and not empty.");
                throw new Exception("The column's name must not be max 15 letters and not empty.");
            }
            this.id = id;
            this.name = name;
            this.limit = limit;
            this.ordinal = ordinal;
            this._tasks = new List<Task>();
        }

        public Column()
        {
            this.tasks = new List<Task>();
        }

        //=====================================================================================================================

        public void ChangeColumnName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                log.Error("The column's name could'nt be empty");
                throw new Exception("The column's name could'nt be empty");
            }
            if (newName.Length > 15)
            {
                log.Error("The column's name could'nt be more more than 15 letters.");
                throw new Exception("The column's name could'nt be more more than 15 letters.");
            }
            //updating the column's name in the object.
            name = newName;
            //updating the column's name in the database.
            update(id, "name", newName);
        }

        public void updatelimit(int limit)
        {
            if (limit < 0)
            {
                log.Error("The limit is negative.");
                throw new Exception("The limit is negative.");
            }
            //the new limit must be bigger than the number of the current tasks in the column.
            if (this.tasks.Count > limit)
            {
                log.Error("The number of th current tasks is higher than the limit.");
                throw new Exception("The number of th current tasks is higher than the limit.");
            }
            this._limit = limit;
            log.Info("The limit is updated.");
            update(id,"limit",limit);
        }

        public void updatetitle(string email, int id, string title)
        {
            try
            {
                bool exist = false;
                //searching for the task with the requested id.
                for (int i = 0; i < tasks.Count; i++)
                {
                    Task task = tasks[i];
                    if (task.id == id)
                    {
                        //updating the title.
                        task.edittitle(email,title);
                        exist = true;
                    }
                }
                //if the task does not exist so we throw an exception.
                if (!exist)
                {
                    log.Error("There is no task with this id.");
                    throw new Exception("There is no task with this id.");
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void updatedescription(string email, int id, string description)
        {
            try
            {
                bool exist = false;
                //searching for the task with the requested id.
                for (int i = 0; i < tasks.Count; i++)
                {
                    Task task = tasks[i];
                    if (task.id == id)
                    {
                        //updating the description.
                        task.editdescription(email,description);
                        exist = true;
                    }
                }
                //if the task does not exist so we throw an exception.
                if (!exist)
                {
                    log.Error("There is no task with this id.");
                    throw new Exception("There is no task with this id.");
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void updateduedate(string email, int id, DateTime duedate)
        {
            try
            {
                bool exist = false;
                //searching for the task with the requested id.
                for (int i = 0; i < tasks.Count; i++)
                {
                    Task task = tasks[i];
                    if (task.id == id)
                    {
                        //updating the duedate.
                        task.editduedate(email,duedate);
                        exist = true;
                    }
                }
                //if the task does not exist so we throw an exception.
                if (!exist)
                {
                    log.Error("There is no task with this id.");
                    throw new Exception("There is no task with this id.");
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void addtask(Task task)
        {
            //if the column is full we can't add a new task to it.
            if (this.tasks.Count == limit)
            {
                log.Error("The column is full.");
                throw new Exception("The column is full.");
            }
            tasks.Add(task);
            log.Info("The task is added.");
            //updating the task's columnid in the task object and in the databse.
            task.columnid = id;
            task.update(task.id,"columnid",task.columnid);
        }

        public Task RemoveTask(int taskId)
        {
            bool exist = false;
            Task newtask = null;
            //searching for the task with the same requested id.
            for (int i = 0; i < this.tasks.Count & !exist; i++)
            {
                if (tasks[i].id == taskId)
                {
                    newtask = tasks[i];
                    tasks.Remove(newtask);
                    exist = true;
                    log.Info("The task has been removed from the old column.");
                }
            }
            //if the task does not exist then we throw an exception.
            if (!exist)
            {
                log.Error("There is no task with this id.");
                throw new Exception("There is no task with this id.");
            }
            return newtask;
        }

        public void AssignTask(int taskId, string emailAssignee)
        {
            try
            {
                bool exist = false;
                //searching for the task with the same requested id.
                foreach (Task task in tasks)
                {
                    if (task.id == taskId)
                    {
                        //upaditng the assigned user email for  the task in the object itself and in the database.
                        task.emailassignee = emailAssignee;
                        task.update(task.id, "emailassignee", emailAssignee);
                        exist = true;
                    }
                }
                //if the task does not exist then we throw an exception.
                if (!exist)
                {
                    log.Error("There is no task with this id.");
                    throw new Exception("There is no task with this id.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteTask(string email, int taskId)
        {
            try
            {
                bool exist = false;
                //searching for the task with the same requested id.
                for (int i=0; i<tasks.Count;i++)
                {
                    Task task = tasks[i];
                    if (task.id == taskId)
                    {
                        //only the assigned user is allowed to delete the task.
                        if(task.emailassignee != email)
                        {
                            log.Error("Only the assigned user allowed to delte the task.");
                            throw new Exception("Only the assigned user allowed to delte the task.");
                        }
                        //deleting the task from the database.
                        tasks.Remove(task);
                        task.Save("delete");
                        exist = true;
                    }
                }
                //if the task does not exist then we throw an exception.
                if (!exist)
                {
                    log.Error("There is no task with this id.");
                    throw new Exception("There is no task with this id.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool isfull()
        {
            return tasks.Count == limit;
        }

        //=====================================================================================================================
        //Implementing PersistedObject interface

        public void import(Data.Column dal)
        {
            this._id = dal.Id;
            this._name = dal.name;
            this._limit = dal.limit;
            this.ordinal = dal.ordinal;
        }

        public void Save(string action)
        {
            if (action == "insert")
            {
                Controls.ColumnControl control = new Controls.ColumnControl();
                control.Insert(this.todal());
            }
            else if (action == "delete")
            {
                DalControl control = new Controls.ColumnControl();
                control.Delete(this.todal());
            }
        }

        public Data.Column todal()
        {
            return new Data.Column(id, name, limit, ordinal, this.boardid);
        }

        //-------------------------------------------
        //Functions for updating the database.

        public void update(int id, string attributeName, string attributeValue)
        {
            DalControl control = new Controls.ColumnControl();
            control.Update(id, attributeName, attributeValue);
        }

        public void update(int id, string attributeName, int attributeValue)
        {
            DalControl control = new Controls.ColumnControl();
            control.Update(id, attributeName, attributeValue);
        }

        //===================================================================================================================
        //helper function for the advancetask method.

        public int place(int taskId)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if(tasks[i].id == taskId)
                {
                    return i;
                }
            }
            return -1;
        }

        //======================================================================================================================
        // getters and setters.

        public int id
        {
            get { return _id; }
            set { this._id = value; }
        }

        public string name
        {
            get { return _name; }
            set { this._name = value; }
        }

        public List<Task> tasks
        {
            get { return _tasks; }
            set { this._tasks = value; }
        }

        public int limit
        {
            get { return _limit; }
            set { this._limit = value; }
        }

        public int ordinal
        {
            get { return _ordinal; }
            set { this._ordinal = value; }
        }
    }
}
