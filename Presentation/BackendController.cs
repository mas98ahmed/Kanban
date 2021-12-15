using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Presentation
{
    public class BackendController
    {
        private IService Service { get; set; }
        public BackendController(IService service)
        {
            this.Service = service;
        }

        public BackendController()
        {
            this.Service = new Service();
            Service.LoadData();
        }

        //=====================================================================================================================================
        //Users Methods.

        public void Register(string email, string password, string nickname)
        {
            Response res = Service.Register(email, password, nickname);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public void Register(string email, string password, string nickname, string hostemail)
        {
            Response res = Service.Register(email, password, nickname, hostemail);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public UserModel Login(string email, string password)
        {
            Response<User> res = Service.Login(email, password);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return new UserModel(this, res.Value.Email, res.Value.Nickname);
        }

        //=========================================================================================================================================================
        //Methods Methods.
        public BoardModel GetBoard(string email)
        {
            Response<Board> res = Service.GetBoard(email);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            string creator = res.Value.emailCreator;
            ObservableCollection<string> columns = new ObservableCollection<string>(res.Value.ColumnsNames);
            return new BoardModel(this, columns, creator, email);
        }

        public ColumnModel GetColumn(string email, int ordinal)
        {
            Response<Column> res = Service.GetColumn(email, ordinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            string columnname = res.Value.Name;
            int limit = res.Value.Limit;
            List<TaskModel> tasks = new List<TaskModel>();
            foreach (Task task in res.Value.Tasks)
            {
                tasks.Add(new TaskModel(this, email, task.Id, task.Title, task.Description, task.emailAssignee, task.DueDate, task.CreationTime));
            }
            return new ColumnModel(this, columnname, limit, tasks, email);
        }

        public ColumnModel AddColumn(UserModel user, string name, int ordinal)
        {
            Response<Column> res = Service.AddColumn(user.Email, ordinal, name);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            List<TaskModel> tasks = new List<TaskModel>();
            foreach (Task task in res.Value.Tasks)
            {
                tasks.Add(new TaskModel(this, user.Email, task.Id, task.Title, task.Description, task.emailAssignee, task.DueDate, task.CreationTime));
            }
            return new ColumnModel(this, res.Value.Name, res.Value.Limit, tasks, user.Email);
        }

        public void RemoveColumn(string email, int columnordinal)
        {
            Response res = Service.RemoveColumn(email, columnordinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public void MoveColumnRight(string email, int ColumnOrdinal)
        {
            Response<Column> res = Service.MoveColumnRight(email, ColumnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public void MoveColumnLeft(string email, int ColumnOrdinal)
        {
            Response<Column> res = Service.MoveColumnLeft(email, ColumnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public void ChangeName(string email, int columnOrdinal, string value)
        {
            Response res = Service.ChangeColumnName(email, columnOrdinal, value);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public void ChangeLimit(string email, int ordinal, int value)
        {
            Response res = Service.LimitColumnTasks(email,ordinal,value);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        //=========================================================================================================================================================
        //Tasks Methods.

        public TaskModel AddTask(UserModel user, DateTime dueDate, string title, string descritpion)
        {
            Response<Task> res = Service.AddTask(user.Email, title, descritpion, dueDate);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return new TaskModel(this, user.Email, res.Value.Id, res.Value.Title, res.Value.Description, res.Value.emailAssignee, res.Value.DueDate, res.Value.CreationTime);
        }

        public void DeleteTask(UserModel user, TaskModel selectedTask)
        {
            Response res = Service.DeleteTask(user.Email, selectedTask.columnordinal, selectedTask.Id);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public void Update(TaskModel selectedTask, DateTime duedate, string title, string description)
        {
            Response res_duedate = Service.UpdateTaskDueDate(selectedTask.EmailAssignee, selectedTask.columnordinal, selectedTask.Id, duedate);
            Response res_title = Service.UpdateTaskTitle(selectedTask.EmailAssignee, selectedTask.columnordinal, selectedTask.Id, title);
            Response res_description = Service.UpdateTaskDescription(selectedTask.EmailAssignee, selectedTask.columnordinal, selectedTask.Id, description);
            if (res_duedate.ErrorOccured || res_title.ErrorOccured || res_description.ErrorOccured)
            {
                throw new Exception(res_duedate + "/n" + res_title + "/n" + res_description);
            }
        }

        public void AssignTask(string email, int columnordinal, int taskid, string emailAssignee)
        {
            Response res = Service.AssignTask(email, columnordinal, taskid, emailAssignee);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        public void AdvanceTask(UserModel user, TaskModel selectedTask)
        {
            Response res = Service.AdvanceTask(user.Email, selectedTask.columnordinal, selectedTask.Id);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
    }
}
