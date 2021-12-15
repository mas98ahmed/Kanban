using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Presentation.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        public ObservableCollection<TaskModel> Tasks { get; set; }
        public int ordinal { get; set; }

        public string Email { get; set; }

        //=================================================================================================================================================================================

        public ColumnModel(BackendController controller, string name, int limit, List<TaskModel> tasks, int ordinal, string email) : base(controller)
        {
            this.Name = name;
            this.Limit = limit;
            this.ordinal = ordinal;
            this.Email = email;
            this.Tasks = new ObservableCollection<TaskModel>(tasks.
                                                Select((t, i) => new TaskModel(controller, email, t.Id, t.Title, t.Descritpion, t.EmailAssignee, t.DueDate, t.CreationTime, ordinal)).ToList());
        }

        public ColumnModel(BackendController controller, string name, int limit, List<TaskModel> tasks, string email) : base(controller)
        {
            this.Tasks = new ObservableCollection<TaskModel>(tasks.
                                                Select((t, i) => new TaskModel(controller, email, t.Id, t.Title, t.Descritpion, t.EmailAssignee, t.DueDate, t.CreationTime, i)).ToList());
            this.Name = name;
            this.Limit = limit;
            this.Email = email;
        }

        public ColumnModel(BackendController controller, ColumnModel column, int ordinal, string email) : this(controller, column.Name, column.Limit, column.Tasks.ToList(), ordinal, email)
        {
        }

        //=========================================================================================================================================================
        //Tasks Methods.

        public void Add(TaskModel task)
        {
            Tasks.Add(task);
        }

        public void DeleteTask(TaskModel selectedTask)
        {
            Tasks.Remove(selectedTask);
        }

        public void Update(int taskId, DateTime dueDate, string title, string descritpion)
        {
            foreach (TaskModel task in Tasks)
            {
                if (task.Id == taskId)
                {
                    task.Update(dueDate, title, descritpion);
                }
            }
        }

        //=========================================================================================================================================================
        //Columns Methods.

        public void MoveLeft()
        {
            for (int i = 0; i < Tasks.Count; i++)
            {
                Tasks[i].columnordinal -= 1;
            }
        }

        public void MoveRight()
        {
            for (int i = 0; i < Tasks.Count; i++)
            {
                Tasks[i].columnordinal += 1;
            }
        }

        //=================================================================================================================================================================================
        //Filtering and Sorting.

        public void Filter(string searching_Word)
        {
            for (int i = 0; i < Tasks.Count; i++)
            {
                if (!Tasks[i].Title.Contains(searching_Word) && !Tasks[i].Descritpion.Contains(searching_Word))
                {
                    Tasks[i].IsVisible = false;
                }
                if (Tasks[i].Title.Contains(searching_Word) || Tasks[i].Descritpion.Contains(searching_Word))
                {
                    Tasks[i].IsVisible = true;
                }
            }
        }

        public void ResetFilter()
        {
            for (int i = 0; i < Tasks.Count; i++)
            {
                Tasks[i].IsVisible = true;
            }
        }

        public void SortTasks()
        {
            List<TaskModel> tasks = Tasks.OrderBy(x => x.DueDate).ToList();
            Tasks.Clear();
            for (int i = 0; i < tasks.Count; i++)
            {
                Tasks.Add(tasks[i]);
            }
        }

        //=========================================================================================================================================================
        //Bindings.

        //helping variable.
        private bool name_working_program = false;//starting to update after the program has been booted
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                try
                {
                    //updating after starting it.
                    if (name_working_program)
                    {
                        Controller.ChangeName(Email,ordinal,value);
                    }
                    _name = value;
                    RaisePropertyChanged("Name");
                    name_working_program = true;
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        //helping variable.
        private bool limit_working_program = false;//starting to update after the program has been booted
        private int _limit;
        public int Limit
        {
            get => _limit;
            set
            {
                try
                {
                    //updating after starting it.
                    if (limit_working_program)
                    {
                        Controller.ChangeLimit(Email, ordinal, value);
                    }
                    _limit = value;
                    RaisePropertyChanged("Name");
                    limit_working_program = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
    }
}
