using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using System;
using System.Linq;
using System.Collections.Generic;
using Data = IntroSE.Kanban.Backend.DataAccessLayer.Objects;
using Controls = IntroSE.Kanban.Backend.DataAccessLayer.Controls;
using Task = IntroSE.Kanban.Backend.BusinessLayer.BoardPackage.Task;


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class BoardControl
    {
        private Dictionary<string, Board> boards;//<email,borad>
        private List<Column> columns;//all the columns in the system.
        private int boards_current_id;//the number of the boards in the system.
        private int columns_current_id;//the number of them columns in the system.
        private int tasks_current_id;//the number of the tasks in the system.
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        // defining a new board control
        public BoardControl()
        {
            boards = new Dictionary<string, Board>();
            columns = new List<Column>();
            this.boards_current_id = 0;
            this.columns_current_id = 0;
            this.tasks_current_id = 0;
        }


        //===================================================================================================================================
        //Functions connected to database.

        // load board data
        public void LoadData()
        {
            Controls.UserControl userscontrol = new Controls.UserControl();//users control for the database.
            List<Data.User> data_users = userscontrol.SelectAllUsers().ToList<Data.User>();//all the users in the database.
            Dictionary<string, int> usersdic = new Dictionary<string, int>();//<user.email,user.boardid>
            for (int i = 0; i < data_users.Count; i++)
            {
                usersdic[data_users[i].email] = data_users[i].boardid;
            }
            //------------------------------------------------------------------------------------
            Controls.TaskControl taskscontrol = new Controls.TaskControl();
            List<Data.Task> data_tasks = taskscontrol.SelectAllTasks().ToList<Data.Task>();
            Dictionary<int, List<Task>> tasksdic = new Dictionary<int, List<Task>>();//<task.columnid,list of tasks that are related to the column with this id>
            //adding the tasks to the dictionary
            for(int i = 0; i < data_tasks.Count; i++)
            {
                if (!tasksdic.ContainsKey(data_tasks[i].columnID))
                {
                    tasksdic[data_tasks[i].columnID] = new List<Task>();
                }
                Task task = new Task();
                task.import(data_tasks[i]);
                tasksdic[data_tasks[i].columnID].Add(task);
                //finding the max task's id number.
                if (this.tasks_current_id <= task.id)
                {
                    this.tasks_current_id = task.id + 1;
                }
            }
            //------------------------------------------------------------------------------------
            Controls.ColumnControl columnscontrol = new Controls.ColumnControl();
            List<Data.Column> data_columns = columnscontrol.SelectAllColumns().ToList<Data.Column>();
            Dictionary<int, List<Column>> columnsdic = new Dictionary<int, List<Column>>();//<column.id,list of columns that are related to the board with this id>
            //adding the columns to the dictionary
            for (int i = 0; i < data_columns.Count; i++)
            {
                if (!columnsdic.ContainsKey(data_columns[i].boardid))
                {
                    columnsdic[data_columns[i].boardid] = new List<Column>();
                }
                Column column = new Column();
                if (tasksdic.ContainsKey(data_columns[i].Id))
                {
                    column.tasks = tasksdic[data_columns[i].Id];
                }
                column.import(data_columns[i]);
                columnsdic[data_columns[i].boardid].Add(column);
                this.columns.Add(column);
                //finding the max column's id number.
                if(this.columns_current_id <= column.id)
                {
                    this.columns_current_id = column.id + 1;
                }
            }
            //sorting every list of columns by the ordinal.
            for (int i = 0; i < columnsdic.Keys.ToList<int>().Count; i++)
            {
                columnsdic[columnsdic.Keys.ToList<int>()[i]] = columnsdic[columnsdic.Keys.ToList<int>()[i]].OrderBy(x => x.ordinal).ToList<Column>();
            }
            this.columns = this.columns.OrderBy(x => x.id).ToList<Column>();
            //------------------------------------------------------------------------------------
            Controls.BoardControl boardscontrol = new Controls.BoardControl();//boards control for the database.
            List<Data.Board> data_boards = boardscontrol.SelectAllBoards().ToList<Data.Board>();//all the boards in the database.
            Dictionary<int, Board> boardsdic = new Dictionary<int, Board>();//<board.id,Board>
            for (int i = 0; i < data_boards.Count; i++)
            {
                boardsdic[data_boards[i].Id] = new Board(columnsdic[data_boards[i].Id]);
                boardsdic[data_boards[i].Id].import(data_boards[i]);
            }
            //------------------------------------------------------------------------------------
            //entering the boards to the system dictionary.
            for(int i = 0; i < usersdic.Keys.ToList<string>().Count; i++)
            {
                this.boards[usersdic.Keys.ToList<string>()[i].ToLower()] = boardsdic[usersdic[usersdic.Keys.ToList<string>()[i]]];
            }
            //------------------------------------------------------------------------------------
            //updating the  boards number.
            this.boards_current_id = data_boards.Count;
        }

        //deleting the data.
        public void DeleteData()
        {
            List<string> deletedboards = boards.Keys.ToList<string>();
            for (int i = 0; i < deletedboards.Count; i++)
            {
                for (int j = 0; j < boards[deletedboards[i]].columns.Count; j++)
                {
                    for (int k = 0; k < boards[deletedboards[i]].columns[j].tasks.Count; k++)
                    {
                        boards[deletedboards[i]].columns[j].tasks[k].Save("delete");
                    }
                }

                for (int j = 0; j < boards[deletedboards[i]].columns.Count; j++)
                {
                    boards[deletedboards[i]].columns[j].Save("delete");
                }

                boards[deletedboards[i]].Save("delete");
            }
            this.boards_current_id = 0;
            this.tasks_current_id = 0;
            this.columns_current_id = 0;
            boards = new Dictionary<string, Board>();
            columns = new List<Column>();
        }

        //===================================================================================================================================

        public Board GetBoard(string email, bool active)
        {
            if (!boards.ContainsKey(email.ToLower()))
            {
                log.Error("There is no user with this email.");
                throw new Exception("There is no user with this email.");
            }
            if (!active)
            {
                log.Error("The user is not active.");
                throw new Exception("The user is not active.");
            }
            return boards[email.ToLower()];
        }

        //===================================================================================================================================
        //Fuctions connected to columns.

        //limit the number of tasks in a column
        public void LimitColumnTasks(string email, int columnOrdinal, int limit, bool active)
        {
            try
            {
                //checking if there is a user with this email.
                if (!boards.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                //Only if the user is active we can update the limit.
                if (!active)
                {
                    log.Error("The user is not active.");
                    throw new Exception("The user is not active.");
                }
                //updating the limit.
                boards[email.ToLower()].LimitColumnTasks(email, columnOrdinal, limit);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // get desired column
        public Column GetColumn(string email, string columnName, bool active)
        {
            try
            {
                //checking if there is a user with this email.
                if (!boards.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                //Only if the user is active we can update the limit.
                if (!active)
                {
                    log.Error("The user is not active.");
                    throw new Exception("The user is not active.");
                }
                //getting the column by its name.
                return boards[email.ToLower()].GetColumn(columnName);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // get desired column
        public Column GetColumn(string email, int columnOrdinal, bool active)
        {
            try
            {
                //checking if there is a user with this email.
                if (!boards.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                //Only if the user is active we can update the limit.
                if (!active)
                {
                    log.Error("The user is not active.");
                    throw new Exception("The user is not active.");
                }
                //getting a column by its id.
                return boards[email.ToLower()].GetColumn(columnOrdinal);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void ChangeColumnName(string email, int columnOrdinal, string newName, bool active)
        {
            try
            {
                //checking if there is a user with this email.
                if (!boards.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                //Only if the user is active the he can change the name of the column
                if (!active)
                {
                    log.Error("The user is not active.");
                    throw new Exception("The user is not active.");
                }
                //changing the column's name.
                boards[email.ToLower()].ChangeColumnName(email, columnOrdinal, newName);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //remove a desired column from board
        public void RemoveColumn(string email, int columnOrdinal, bool active)
        {
            try
            {
                //checking if there is a user with this email.
                if (!boards.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                //Only if the user is active the he can remove a column.
                if (!active)
                {
                    log.Error("The user is not active.");
                    throw new Exception("The user is not active.");
                }
                //the index of the column that we remove from the board.
                int place = boards[email.ToLower()].RemoveColumn(email, columnOrdinal);
                //updating the columns number in the system after deleting the column.
                this.columns_current_id--;
                //removing the column from the the list.
                this.columns.RemoveAt(place);
                //updating all the columns by changing everyone order in the database and the object.
                for (int i = place; i < this.columns.Count; i++)
                {
                    this.columns[i].update(columns[i].id, "ID", columns[i].id-1);
                    this.columns[i].id--;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //add a desired column to board
        public Column AddColumn(string email, int columnOrdinal, string Name, bool active)
        {
            try
            {
                //checking if there is a user with this email.
                if (!boards.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                //Only if the user is active the he can add a column
                if (!active)
                {
                    log.Error("The user is not active.");
                    throw new Exception("The user is not active.");
                }
                //adding a new column.
                boards[email.ToLower()].AddColumn(email, columnOrdinal, Name, this.columns_current_id);
                //updating the columns number in the system after adding the new column. 
                this.columns_current_id += 1;
                //adding the new column to the list
                this.columns.Add(boards[email.ToLower()].columns[columnOrdinal]);
                return boards[email.ToLower()].columns[columnOrdinal];
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // move column to the right
        public Column MoveColumnRight(string email, int columnOrdinal, bool active)
        {
            try
            {
                //checking if there is a user with this email.
                if (!boards.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                //Only if the user is active the he can move the column right.
                if (!active)
                {
                    log.Error("The user is not active.");
                    throw new Exception("The user is not active.");
                }
                //moving the column right.
                boards[email.ToLower()].MoveColumnRight(email, columnOrdinal);
                return boards[email.ToLower()].columns[columnOrdinal + 1];
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // move column to the left
        public Column MoveColumnLeft(string email, int columnOrdinal, bool active)
        {
            try
            {
                //checking if there is a user with this email.
                if (!boards.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                //Only if the user is active the he can move the column left.
                if (!active)
                {
                    log.Error("The user is not active.");
                    throw new Exception("The user is not active.");
                }
                //moving the column left.
                boards[email.ToLower()].MoveColumnLeft(email, columnOrdinal);
                return boards[email.ToLower()].columns[columnOrdinal - 1];
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //===================================================================================================================================
        //Funtions connected to task.

        // add a new task to the board
        public Task AddTask(string email, string title, string description, DateTime dueDate, bool active)
        {
            try
            {
                //checking if there is a user with this email.
                if (!boards.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                //Only if the user is active the he can add a task.
                if (!active)
                {
                    log.Error("The user is not active.");
                    throw new Exception("The user is not active.");
                }
                //adding a new task.
                Task task = boards[email.ToLower()].AddTask(this.tasks_current_id, email, title, description, dueDate);
                //updating the number of the task in the system.
                tasks_current_id += 1;
                return task;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteTask(string email, int columnOrdinal, int taskId, bool active)
        {
            //checking if there is a user with this email.
            if (!boards.ContainsKey(email.ToLower()))
            {
                log.Error("There is no user with this email.");
                throw new Exception("There is no user with this email.");
            }
            //Only if the user is active the he can delete a task.
            if (!active)
            {
                log.Error("The user is not active.");
                throw new Exception("The user is not active.");
            }
            //deleting the task.
            boards[email.ToLower()].DeleteTask(email, columnOrdinal, taskId);
        }

        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee, bool active)
        {
            //checking if there is a user with this email.
            if (!boards.ContainsKey(email.ToLower()) || !boards.ContainsKey(emailAssignee.ToLower()))
            {
                log.Error("There is no user with this email.");
                throw new Exception("There is no user with this email.");
            }
            //Only if the user is active the he can assign a task.
            if (!active)
            {
                log.Error("The user is not active.");
                throw new Exception("The user is not active.");
            }
            //The task will be assigned to the user this emailassignee Only if the he is connected to the board that has this task.
            if(boards[emailAssignee] != boards[email])
            {
                log.Error("The user with this email is not connected to the compatible board.");
                throw new Exception("The user with this email is not connected to the compatible board.");
            }
            //assigning the task.
            boards[email.ToLower()].AssignTask(email, columnOrdinal, emailAssignee, taskId);
        }

        // update the duedate of an existing task
        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate, bool active)
        {
            try
            {
                //checking if there is a user with this email.
                if (!boards.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                //Only if the user is active the he can update the duedate of a task.
                if (!active)
                {
                    log.Error("The user is not active.");
                    throw new Exception("The user is not active.");
                }
                //update the task's duedate. 
                boards[email.ToLower()].UpdateTaskDueDate(email,columnOrdinal, taskId, dueDate);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // update the title of an existing task
        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title, bool active)
        {
            try
            {
                //checking if there is a user with this email.
                if (!boards.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                //Only if the user is active the he can update the title of a task.
                if (!active)
                {
                    log.Error("The user is not active.");
                    throw new Exception("The user is not active.");
                }
                //updating the task's title.
                boards[email.ToLower()].UpdateTaskTitle(email, columnOrdinal, taskId, title);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // update the description of an existing task
        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description, bool active)
        {
            try
            {
                //checking if there is a user with this email.
                if (!boards.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                //Only if the user is active the he can update the description of a task.
                if (!active)
                {
                    log.Error("The user is not active.");
                    throw new Exception("The user is not active.");
                }
                //updating the task's description.
                boards[email.ToLower()].UpdateTaskDescription(email, columnOrdinal, taskId, description);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // advance task
        public void AdvanceTask(string email, int columnOrdinal, int taskId, bool active)
        {
            try
            {
                //checking if there is a user with this email.
                if (!boards.ContainsKey(email.ToLower()))
                {
                    log.Error("There is no user with this email.");
                    throw new Exception("There is no user with this email.");
                }
                //Only if the user is active the he can advance the task.
                if (!active)
                {
                    log.Error("The user is not active.");
                    throw new Exception("The user is not active.");
                }
                //advancing the task.
                boards[email.ToLower()].AdvanceTask(email,columnOrdinal, taskId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        //=====================================================================================================================================
        //Funcions for registering help.

        // add new board
        public void AddBoard(string email)
        {
            //defining a new column by default.
            boards[email.ToLower()] = new Board(this.boards_current_id,email, columns_current_id);
            this.boards_current_id++;
            //updating the number of the current columns in the system.
            this.columns_current_id += 3;
            //adding the new three columns to the columns list.
            this.columns.Add(boards[email.ToLower()].columns[0]);
            this.columns.Add(boards[email.ToLower()].columns[1]);
            this.columns.Add(boards[email.ToLower()].columns[2]);
        }

        public void AddUserToBoard(string email, string password, string nickname, string emailhost)
        {
            //checking if there is a user with this email.
            if (!boards.ContainsKey(emailhost.ToLower()))
            {
                log.Error("There is no host email such as that.");
                throw new Exception("There is no host email such as that.");
            }
            //connecting the new user to an existed column.
            boards[email.ToLower()] = boards[emailhost.ToLower()];
        }
    }
}
