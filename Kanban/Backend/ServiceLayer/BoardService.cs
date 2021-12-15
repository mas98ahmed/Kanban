using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using B = IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class BoardService
    {
        private BoardControl c;

        public BoardService()
        {
            this.c = new BoardControl();
        }


        public Response LoadData()
        {
            try
            {
                c.LoadData();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response DeleteData()
        {
            try
            {
                c.DeleteData();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<Board> GetBoard(string email, bool active)
        {
            try
            {
                B.Board board = c.GetBoard(email, active);
                List<string> columns = new List<string>();
                for(int i = 0; i < board.columns.Count; i++)
                {
                    columns.Add(board.columns[i].name);
                }
                Board newboard = new Board(columns,board.emailhost);
                return new Response<Board>(newboard);
            }
            catch (Exception e)
            {
                return new Response<Board>(e.Message);
            }
        }

        public Response LimitColumnTasks(string email, int columnOrdinal, int limit, bool active)
        {
            try
            {
                c.LimitColumnTasks(email, columnOrdinal, limit, active);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee, bool active)
        {
            try
            {
                c.AssignTask(email, columnOrdinal, taskId, emailAssignee, active);
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate, bool active)
        {
            try
            {
                //converting a task(Business Layer) to a task(Service Layer).
                B.Task taskb = c.AddTask(email, title, description, dueDate, active);
                Task task = new Task(taskb.id, taskb.creationtime, taskb.duedate, taskb.title, taskb.description,taskb.emailassignee);
                //the response
                Response<Task> response = new Response<Task>(task);
                return response;
            }
            catch (Exception e)
            {
                return new Response<Task>(e.Message);
            }
        }

        public Response DeleteTask(string email, int columnOrdinal, int taskId, bool active)
        {
            try
            {
                c.DeleteTask(email, columnOrdinal, taskId, active);
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate, bool active)
        {
            try
            {
                c.UpdateTaskDueDate(email, columnOrdinal, taskId, dueDate, active);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title, bool active)
        {
            try
            {
                c.UpdateTaskTitle(email, columnOrdinal, taskId, title, active);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description, bool active)
        {
            try
            {
                c.UpdateTaskDescription(email, columnOrdinal, taskId, description, active);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response AdvanceTask(string email, int columnOrdinal, int taskId, bool active)
        {
            try
            {
                c.AdvanceTask(email, columnOrdinal, taskId, active);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<Column> GetColumn(string email, string columnName, bool active)
        {
            try
            {
                //converting a column(Business Layer) to a column(Service Layer).
                B.Column column = c.GetColumn(email, columnName, active);
                List<Task> tasks = new List<Task>();
                foreach (B.Task task in column.tasks)
                {
                    Task newtask = new Task(task.id, task.creationtime, task.duedate, task.title, task.description, task.emailassignee);
                    tasks.Add(newtask);
                }
                Column newcolumn = new Column(tasks, column.name, column.limit);
                //The response.
                return new Response<Column>(newcolumn);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
        }

        public Response<Column> GetColumn(string email, int columnOrdinal, bool active)
        {
            try
            {
                //converting a column(Business Layer) to a column(Service Layer).
                B.Column column = c.GetColumn(email, columnOrdinal, active);
                List<Task> tasks = new List<Task>();
                foreach (B.Task task in column.tasks)
                {
                    Task newtask = new Task(task.id, task.creationtime, task.duedate, task.title, task.description, task.emailassignee);
                    tasks.Add(newtask);
                }
                Column newcolumn = new Column(tasks, column.name, column.limit);
                //The response.
                return new Response<Column>(newcolumn);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
        }

        public Response ChangeColumnName(string email, int columnOrdinal, string newName, bool active)
        {
            try
            {
                c.ChangeColumnName(email, columnOrdinal, newName, active);
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response RemoveColumn(string email, int columnOrdinal, bool active)
        {
            try
            {
                c.RemoveColumn(email, columnOrdinal, active);
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<Column> AddColumn(string email, int columnOrdinal, string Name, bool active)
        {
            try
            {
                //converting a  column(Business Layer) to a column(Service Layer).
                B.Column column = c.AddColumn(email, columnOrdinal, Name, active);
                List<Task> tasks = new List<Task>();
                foreach (B.Task task in column.tasks)
                {
                    Task newtask = new Task(task.id, task.creationtime, task.duedate, task.title, task.description, task.emailassignee);
                    tasks.Add(newtask);
                }
                Column newcolumn = new Column(tasks, column.name, column.limit);
                //The response.
                return new Response<Column>(newcolumn);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
        }

        public Response<Column> MoveColumnRight(string email, int columnOrdinal, bool active)
        {
            try
            {
                //converting a  column(Business Layer) to a column(Service Layer).
                B.Column column = c.MoveColumnRight(email, columnOrdinal, active);
                List<Task> tasks = new List<Task>();
                foreach (B.Task task in column.tasks)
                {
                    Task newtask = new Task(task.id, task.creationtime, task.duedate, task.title, task.description, task.emailassignee);
                    tasks.Add(newtask);
                }
                Column newcolumn = new Column(tasks, column.name, column.limit);
                //The response.
                return new Response<Column>(newcolumn);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
        }

        public Response<Column> MoveColumnLeft(string email, int columnOrdinal, bool active)
        {
            try
            {
                //converting a  column(Business Layer) to a column(Service Layer).
                B.Column column = c.MoveColumnLeft(email, columnOrdinal, active);
                List<Task> tasks = new List<Task>();
                foreach (B.Task task in column.tasks)
                {
                    Task newtask = new Task(task.id, task.creationtime, task.duedate, task.title, task.description, task.emailassignee);
                    tasks.Add(newtask);
                }
                Column newcolumn = new Column(tasks, column.name, column.limit);
                //The response.
                return new Response<Column>(newcolumn);
            }
            catch (Exception e)
            {
                return new Response<Column>(e.Message);
            }
        }

        //===================================================================================================================================
        //Functions For registering helping.
        //those functions are used for helping in the design pattern.
        
        //The function for adding board for new host user.
        internal Response AddBoard(string email)
        {
            c.AddBoard(email);
            return new Response();
        }
        //The function for adding assignee user to a existed board.
        internal Response AddUserToBoard(string email, string password, string nickname, string emailhost)
        {
            try
            {
                c.AddUserToBoard(email, password, nickname, emailhost);
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }

        //===================================================================================================================================
    }
}
