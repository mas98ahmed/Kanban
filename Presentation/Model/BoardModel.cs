using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class BoardModel : NotifiableModelObject
    {
        public string Email { get; set; }
        public ObservableCollection<ColumnModel> Columns { get; set; }

        //=======================================================================================================================================================================

        public BoardModel(BackendController controller, UserModel user) : base(controller)
        {
            this.Email = user.Email;
            this.HostEmail = controller.GetBoard(Email).HostEmail;
            Columns = new ObservableCollection<ColumnModel>(controller.GetBoard(Email).Columns.
                                                            Select((c, i) => new ColumnModel(controller, controller.GetColumn(Email, c.ordinal), i, Email)).ToList());
        }

        public BoardModel(BackendController controller, ObservableCollection<string> columns, string emailhost, string email) : base(controller)
        {
            this.Email = email;
            this.HostEmail = emailhost;
            Columns = new ObservableCollection<ColumnModel>(columns.
                                                            Select((c, i) => new ColumnModel(controller, controller.GetColumn(Email, i), i, Email)).ToList());
        }

        //=======================================================================================================================================================================
        //columns methods.

        public void AddColumn(UserModel user, string name, int ordinal)
        {
            ColumnModel column = Controller.AddColumn(user, name, ordinal);
            Columns.Insert(ordinal, column);
            for (int i = ordinal + 1; i < Columns.Count; i++)
            {
                Columns[i].ordinal = i;
                for (int j = 0; j < Columns[i].Tasks.Count; j++)
                {
                    Columns[i].Tasks[j].columnordinal = i;
                }
            }
        }

        public void RemoveColumn(int columnordinal)
        {
            Controller.RemoveColumn(Email, columnordinal);
            ColumnModel removed_column = Columns[columnordinal];
            Columns.Remove(removed_column);
            for (int i = columnordinal; i < Columns.Count; i++)
            {
                Columns[i].ordinal = i;
            }
            if (columnordinal == 0)
            {
                removed_column.MoveRight();
            }
            else
            {
                removed_column.MoveLeft();
            }
        }

        public void MoveColumnLeft(int ColumnOrdinal)
        {
            Controller.MoveColumnLeft(Email, ColumnOrdinal);
            //updating the columnordinal in the tasks.
            Columns[ColumnOrdinal].MoveLeft();
            Columns[ColumnOrdinal - 1].MoveRight();
            //start switching.
            ColumnModel tmp = Columns[ColumnOrdinal - 1];
            //updating the ordinal of the columns.
            Columns[ColumnOrdinal].ordinal -= 1;
            Columns[ColumnOrdinal - 1].ordinal += 1;
            //switching them.
            Columns[ColumnOrdinal - 1] = Columns[ColumnOrdinal];
            Columns[ColumnOrdinal] = tmp;
        }

        public void MoveColumnRight(int ColumnOrdinal)
        {
            Controller.MoveColumnRight(Email, ColumnOrdinal);
            //updating the columnordinal in the tasks.
            Columns[ColumnOrdinal].MoveRight();
            Columns[ColumnOrdinal + 1].MoveLeft();
            //start switching.
            ColumnModel tmp = Columns[ColumnOrdinal + 1];
            //updating the ordinal of the columns.
            Columns[ColumnOrdinal].ordinal += 1;
            Columns[ColumnOrdinal + 1].ordinal -= 1;
            //switching them.
            Columns[ColumnOrdinal + 1] = Columns[ColumnOrdinal];
            Columns[ColumnOrdinal] = tmp;
        }

        //=======================================================================================================================================================================
        //Tasks methods.

        public void AddTask(UserModel user, DateTime dueDate, string title, string description)
        {
            TaskModel task = Controller.AddTask(user, dueDate, title, description);
            Columns[task.columnordinal].Add(task);
        }

        public void DeleteTask(UserModel user, TaskModel selectedTask)
        {
            Controller.DeleteTask(user, selectedTask);
            Columns[selectedTask.columnordinal].DeleteTask(selectedTask);
        }

        public void AdvanceTask(UserModel user, TaskModel selectedTask)
        {
            Controller.AdvanceTask(user, selectedTask);
            Columns[selectedTask.columnordinal].Tasks.Remove(selectedTask);
            selectedTask.columnordinal += 1;
            Columns[selectedTask.columnordinal].Tasks.Add(selectedTask);
        }

        public void Update(TaskModel task, DateTime dueDate, string title, string description)
        {
            Controller.Update(task, dueDate, title, description);
            Columns[task.columnordinal].Update(task.Id, dueDate, title, description);
        }

        public void AssignTask(string emailAssignee, TaskModel task)
        {
            try
            {
                Controller.AssignTask(Email, task.columnordinal, task.Id, emailAssignee);
                task.EmailAssignee = emailAssignee;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //=======================================================================================================================================================================
        //Filtering and sorting methods.

        public void Filter(string searching_Word)
        {
            for (int i = 0; i < Columns.Count; i++)
            {
                Columns[i].Filter(searching_Word);
            }
        }

        public void ResetFilter()
        {
            for (int i = 0; i < Columns.Count; i++)
            {
                Columns[i].ResetFilter();
            }
        }

        public void SortTasks()
        {
            for (int i = 0; i < Columns.Count; i++)
            {
                Columns[i].SortTasks();
            }
        }

        //=======================================================================================================================================================================
        //Bindings.

        private string _hostemail;
        public string HostEmail
        {
            get => _hostemail;
            set
            {
                _hostemail = value;
                RaisePropertyChanged("HostEmail");
            }
        }
    }
}
