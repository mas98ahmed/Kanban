using System;
using System.Collections.Generic;
using System.Linq;
using Data =IntroSE.Kanban.Backend.DataAccessLayer.Objects;
using Controls = IntroSE.Kanban.Backend.DataAccessLayer.Controls;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Runtime.CompilerServices;


namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    class Board : PersistedObject<Data.Board>
    {
        private int _id;
        private string _emailhost;//The creator of the board.
        private List<Column> _columns;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // define a new board
        public Board(int id, string email, int column_current_id)
        {
            this.id = id;
            this.emailhost = email;
            _columns = new List<Column>();
            //the default case (defining three columns in the board).
            columns.Add(new Column(column_current_id, "backlog", 100, 0, id));
            columns.Add(new Column(column_current_id + 1, "in progress", 100, 1, id));
            columns.Add(new Column(column_current_id + 2, "done", 100, 2, id));
            Save("insert");
        }

        public Board(List<Column> columns)
        {
            this._columns = columns;
        }


        //=====================================================================================================================

        public void LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            if (columnOrdinal>columns.Count-1 || columnOrdinal < 0)//checking if the columnOrdinal is the range of the columns ordinal
            {
                log.Error("The user does not have a column in this ordinal.");
                throw new Exception("The user does not have a column in this ordinal.");
            }
            if (emailhost != email)//Only the host who can change the limit of a column.
            {
                log.Error("Only the host who has the permission to limit the column.");
                throw new Exception("Only the host who has the permission to limit the column.");
            }
            columns[columnOrdinal].updatelimit(limit);
        }
    
        public Task AddTask(int id, string email, string title, string description, DateTime dueDate)
        {
            try
            {
                if (columns[0].isfull())
                {
                    log.Error("Can't add a new task the column is full.");
                    throw new Exception("Can't add a new task the column is full.");
                }
                //defining a new task assigned to the creator by default.
                Task task = new Task(id, email, DateTime.Now, title, description, dueDate, columns[0].id);
                columns[0].tasks.Add(task);
                return task;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            //checking if the columnOrdinal is the range of the columns ordinal.
            if (columnOrdinal > columns.Count - 1 || columnOrdinal < 0)
            {
                log.Error("The user does not have a column in this ordinal.");
                throw new Exception("The user does not have a column in this ordinal.");
            }
            //cant change the task details if it reached the last column.
            if (columns.Count - 1 == columnOrdinal)
            {
                log.Error("The task in the last column.");
                throw new Exception("The task in the last column.");
            }
            //updating the duedate.
            columns[columnOrdinal].updateduedate(email, taskId,dueDate);
        }

        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            //checking if the columnOrdinal is the range of the columns ordinal.
            if (columnOrdinal > columns.Count - 1 || columnOrdinal < 0)
            {
                log.Error("The user does not have a column in this ordinal.");
                throw new Exception("The user does not have a column in this ordinal.");
            }
            //cant change the task details if it reached the last column.
            if (columns.Count - 1 == columnOrdinal)
            {
                log.Error("The task in the last column.");
                throw new Exception("The task in the last column.");
            }
            //updating the title.
            columns[columnOrdinal].updatetitle(email, taskId, title);
        }

        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            //checking if the columnOrdinal is the range of the columns ordinal.
            if (columnOrdinal > columns.Count - 1 || columnOrdinal < 0)
            {
                log.Error("The user does not have a column in this ordinal.");
                throw new Exception("The user does not have a column in this ordinal.");
            }
            //cant change the task details if it reached the last column.
            if (columns.Count - 1 == columnOrdinal)
            {
                log.Error("The task in the last column.");
                throw new Exception("The task in the last column.");
            }
            //updating the description.
            columns[columnOrdinal].updatedescription(email,taskId, description);
        }

        public void AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            //checking if the columnOrdinal is the range of the columns ordinal.
            if (columnOrdinal > columns.Count - 1 || columnOrdinal < 0)
            {
                log.Error("The user does not have a column in this ordinal.");
                throw new Exception("The user does not have a column in this ordinal.");
            }
            //Only the assigned user can advance the task.
            if(columns[columnOrdinal].place(taskId)!= -1 && columns[columnOrdinal].tasks[columns[columnOrdinal].place(taskId)].emailassignee != email)
            {
                log.Error("Only the assignee has the permission to advance the task.");
                throw new Exception("Only the assignee has the permission to advance the task.");
            }
            //can't move the task in the last column cause there is no way to go!!!!
            if (columns.Count - 1 == columnOrdinal)
            {
                log.Error("The task in the last column.");
                throw new Exception("The task in the last column.");
            }
            //can't move the task to the next column if the next column is full with tasks.
            if (columns[columnOrdinal + 1].isfull())
            {
                log.Error("The next column is full.");
                throw new Exception("The next column is full.");
            }
            //removing the task from the current task.
            Task task = columns[columnOrdinal].RemoveTask(taskId);
            //adding the task to next column
            columns[columnOrdinal + 1].addtask(task);
        }

        public Column GetColumn(string columnName)
        {
            for(int i = 0; i < columns.Count; i++)
            {
                //searching for the column with the same name.
                if (columns[i].name == columnName)
                {
                    return columns[i];
                }
            }

            //Running just if there is no column with the same name.
            log.Error("There is no column with the same name.");
            throw new Exception("There is no column with the same name.");
        }

        public Column GetColumn(int columnOrdinal)
        {
            //checking if the columnOrdinal is the range of the columns ordinal.
            if (columnOrdinal > columns.Count - 1 || columnOrdinal < 0)
            {
                log.Error("The user does not have a column in this ordinal.");
                throw new Exception("The user does not have a column in this ordinal.");
            }
            return columns[columnOrdinal];
        }

        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            //checking if the columnOrdinal is the range of the columns ordinal.
            if (columnOrdinal > columns.Count - 1 || columnOrdinal < 0)
            {
                log.Error("The user does not have a column in this ordinal.");
                throw new Exception("The user does not have a column in this ordinal.");
            }
            //Only the host can change the column's name.
            if (email != emailhost)
            {
                log.Error("Only the board creator can change the names of the columns.");
                throw new Exception("Only the board creator can change the names of the columns.");
            }
            //updating the column's name in the object.
            columns[columnOrdinal].ChangeColumnName(newName);
        }

        public int RemoveColumn(string email, int columnOrdinal)
        {
            int output;
            //checking if the columnOrdinal is the range of the columns ordinal.
            if (columns.Count <= columnOrdinal || columns.Count < 0)
            {
                log.Error("The board doesn't have a column in this ordinal.");
                throw new Exception("The board doesn't have a column in this ordinal.");
            }
            //Only the host can change the column's name.
            if (emailhost != email)
            {
                log.Error("Only the creator has the permission to remove this column.");
                throw new Exception("Only the creator has the permission to remove this column.");
            }
            //the min columns number is 2 and there is no way to be more less.
            if (columns.Count == 2)
            {
                log.Error("The board has two columns.");
                throw new Exception("The board has two columns.");
            }
            if (columnOrdinal == 0)
            {
                //in the case that the column that we wanna remove is the most left one then we have to check if the number of 
                //the tasks that we wanna add plus the current tasks of the right column of the column that we wanna remove is 
                //more than the limit.
                if (columns[columnOrdinal+1].tasks.Count + columns[columnOrdinal].tasks.Count > columns[columnOrdinal + 1].limit)
                {
                    log.Error("Can't move the tasks.");
                    throw new Exception("Can't move the tasks.");
                }
                //Moving all the tasks to right column of the removed column.
                for (int i=0; i< columns[0].tasks.Count; i++)
                {
                    columns[1].tasks.Add(columns[0].tasks[i]);
                }
                output = columns[0].id;
                //deleting the column from the database and from the object.
                columns[0].Save("delete");
                columns.Remove(columns[0]);
                //updating the ordinal of all the columns on the left of the removed column.
                for (int i = columnOrdinal; i < columns.Count; i++)
                {
                    columns[i].ordinal = i;
                    columns[i].update(columns[i].id, "ordinal", i);
                }
            }
            else
            {
                //in the other case that the column that we wanna remove is not the most left one then we have to check if the number of 
                //the tasks that we wanna add plus the current tasks of the left column of the column that we wanna remove is 
                //more than the limit.
                if (columns[columnOrdinal - 1].tasks.Count + columns[columnOrdinal].tasks.Count > columns[columnOrdinal - 1].limit)
                {
                    log.Error("Can't move the tasks.");
                    throw new Exception("Can't move the tasks.");
                }
                //Moving all the tasks to left column of the removed column.
                for (int i = 0; i < columns[columnOrdinal].tasks.Count; i++)
                {
                    columns[columnOrdinal - 1].tasks.Add(columns[columnOrdinal].tasks[i]);
                }
                output = columns[columnOrdinal].id;
                //deleting the column from the database and from the object.
                columns[columnOrdinal].Save("delete");
                columns.Remove(columns[columnOrdinal]);
                //updating the ordinal of all the columns on the left of the removed column.
                for (int i = columnOrdinal; i < columns.Count; i++)
                {
                    columns[i].ordinal = i;
                    columns[i].update(columns[i].id, "ordinal", i);
                }
            }
            return output;
        }

        public void DeleteTask(string email, int columnOrdinal, int taskId)
        {
            //checking if the columnOrdinal is the range of the columns ordinal.
            if (columns.Count <= columnOrdinal || columns.Count < 0)
            {
                log.Error("The board doesn't have a column in this ordinal.");
                throw new Exception("The board doesn't have a column in this ordinal.");
            }
            //deleting the task.
            columns[columnOrdinal].DeleteTask(email, taskId);
        }

        public void AssignTask(string email, int columnOrdinal, string emailAssignee, int taskId)
        {
            //checking if the columnOrdinal is the range of the columns ordinal.
            if (columns.Count <= columnOrdinal || columns.Count < 0)
            {
                log.Error("The board doesn't have a column in this ordinal.");
                throw new Exception("The board doesn't have a column in this ordinal.");
            }
            //Only the host is allowed to assign tasks to other users related to the board.
            if(emailhost != email)
            {
                log.Error("Only the board creator has the permission to assign tasks to other users.");
                throw new Exception("Only the board creator has the permission to assign tasks to other users.");
            }
            //assigning the task to other user.
            columns[columnOrdinal].AssignTask(taskId, emailAssignee);
        }

        public void AddColumn(string email, int columnOrdinal, string Name, int id)
        {
            //checking if the columnOrdinal is the range of the columns ordinal.
            if (columns.Count < columnOrdinal || columns.Count < 0)
            {
                log.Error("The board doesn't have a column in this ordinal.");
                throw new Exception("The board doesn't have a column in this ordinal.");
            }
            //Only the host can change the column's name.
            if (emailhost != email)
            {
                log.Error("Only the creator has the permission to remove this column.");
                throw new Exception("Only the creator has the permission to remove this column.");
            }
            //checking if there is no column with the same name of hte new column.
            for (int i = 0; i < columns.Count; i++)
            {
                if(columns[i].name == Name)
                {
                    log.Error("The name is already exists.");
                    throw new Exception("The name is already exists.");
                }
            }
            //defining a new column.
            Column newcolumn = new Column(id, Name, 100, columnOrdinal, this.id);
            //adding the new column to the board.
            columns.Add(newcolumn);
            //Moving all the column that there index is bigger than the columnOrdinal to more one right index.  
            for (int i = columns.Count - 1; i > columnOrdinal; i--)
            {
                columns[i] = columns[i - 1];
                columns[i].ordinal = i;
                //updating the new columns order in the database.
                columns[i].update(columns[i].id, "ordinal", i);
            }
            columns[columnOrdinal] = newcolumn;
        }

        //For Unit Tests.
        public void AddColumn(string email, int columnOrdinal, string Name, Column newcolumn)
        {
            //checking if the columnOrdinal is the range of the columns ordinal.
            if (columns.Count < columnOrdinal || columns.Count < 0)
            {
                log.Error("The board doesn't have a column in this ordinal.");
                throw new Exception("The board doesn't have a column in this ordinal.");
            }
            //Only the host can change the column's name.
            if (emailhost != email)
            {
                log.Error("Only the creator has the permission to remove this column.");
                throw new Exception("Only the creator has the permission to remove this column.");
            }
            //checking if there is no column with the same name of hte new column.
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].name == Name)
                {
                    log.Error("The name is already exists.");
                    throw new Exception("The name is already exists.");
                }
            }

            //adding the new column to the board.
            columns.Add(newcolumn);
            //Moving all the column that there index is bigger than the columnOrdinal to more one right index.  
            for (int i = columns.Count - 1; i > columnOrdinal; i--)
            {
                columns[i] = columns[i - 1];
                columns[i].ordinal = i;
                //updating the new columns order in the database.
                columns[i].update(columns[i].id, "ordinal", i);
            }
            columns[columnOrdinal] = newcolumn;
        }

        // moves a requested column to the right
        public void MoveColumnRight(string email, int columnOrdinal)
        {
            // checking if the columnOrdinal is in the range of the indexes
            if (columns.Count <= columnOrdinal || columns.Count < 0)
            {
                log.Error("The board doesn't have a column in this ordinal.");
                throw new Exception("The board doesn't have a column in this ordinal.");
            }
            //checking if the email that we got is the host.
            if (emailhost != email)
            {
                log.Error("Only the host who has the permission to move the column.");
                throw new Exception("Only the host who has the permission to move the column.");
            }
            //Can't move right the most right column.
            if (columns.Count - 1 == columnOrdinal)
            {
                log.Error("The column is the last.");
                throw new Exception("The column is the last.");
            }
            //Switching the ordered column with the column on the right of it.
            Column tmp = columns[columnOrdinal];
            //updating the order of the column on the right by moving it left.
            columns[columnOrdinal] = columns[columnOrdinal + 1];
            columns[columnOrdinal].ordinal = columnOrdinal;
            //Updating the database.
            columns[columnOrdinal].update(columns[columnOrdinal].id, "ordinal", columnOrdinal);
            //updating the order of the requested column by moving it right.
            columns[columnOrdinal + 1] = tmp;
            columns[columnOrdinal+1].ordinal = columnOrdinal+1;
            //Updating the database.
            columns[columnOrdinal+1].update(columns[columnOrdinal+1].id, "ordinal", columnOrdinal+1);
        }
        
        // moves a requested column to the left
        public void MoveColumnLeft(string email, int columnOrdinal)
        {
            // checking if the columnOrdinal is in the range of the indexes
            if (columns.Count <= columnOrdinal || columns.Count < 0)
            {
                log.Error("The board doesn't have a column in this ordinal.");
                throw new Exception("The board doesn't have a column in this ordinal.");
            }
            //checking if the email that we got is the host.
            if (emailhost != email)
            {
                log.Error("Only the host who has the permission to move the column.");
                throw new Exception("Only the host who has the permission to move the column.");
            }
            //Can't move left the most left column.
            if (0 == columnOrdinal)
            {
                log.Error("The column is the first.");
                throw new Exception("The column is the first.");
            }

            //Switching the columns.
            Column tmp = columns[columnOrdinal];
            //updating the order of the column on the left by moving it right.
            columns[columnOrdinal] = columns[columnOrdinal - 1];
            columns[columnOrdinal].ordinal = columnOrdinal;
            //Updating the database.
            columns[columnOrdinal].update(columns[columnOrdinal].id, "ordinal", columnOrdinal);
            //updating the order of the requested column by moving it left.
            columns[columnOrdinal - 1] = tmp;
            columns[columnOrdinal - 1].ordinal = columnOrdinal - 1;
            //Updating the database.
            columns[columnOrdinal - 1].update(columns[columnOrdinal - 1].id, "ordinal", columnOrdinal - 1);
        }

        //=====================================================================================================================
        //Implementing PersistedObject interface

        public Data.Board todal()
        {
            return new Data.Board(id,emailhost);
        }

        public void import(Data.Board dal)
        {
            this.id = dal.Id;
            this.emailhost = dal.emailhost;
        }

        public void Save(string action)
        {
            if (action == "insert")
            {
                Controls.BoardControl control = new Controls.BoardControl();
                control.Insert(this.todal());
            }
            else if (action == "delete")
            {
                DalControl control = new Controls.BoardControl();
                control.Delete(this.todal());
            }
        }

        //=========================================================================================================================
        //getters and setters

        public int id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        public string emailhost
        {
            get { return this._emailhost; }
            set { this._emailhost = value; }
        }

        public List<Column> columns
        {
            get { return this._columns; }
        }
    }   
}
