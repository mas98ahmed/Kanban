using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.Model
{
    public class TaskModel : NotifiableModelObject
    {

        public int columnordinal { get; set; }
        //current User.
        public string Email { get; set; }

        public TaskModel(BackendController controller, string email, int id, string title, string description, string emailassignee,DateTime duedate, DateTime creationtime, int ordinal) : base(controller)
        {
            this.Id = id;
            this.Email = email;
            this.Title = title;
            this.Descritpion = description;
            this.EmailAssignee = emailassignee;
            this.DueDate = duedate;
            this.CreationTime = creationtime;
            this.columnordinal = ordinal;
            //checking if the task is down or if 75% of its time has passed.
            var total = (DueDate - CreationTime).TotalSeconds;
            var percentage = (DateTime.Now - CreationTime).TotalSeconds * 100 / total;
            int result = (int)percentage;
            _TaskBackground = DateTime.Compare(DateTime.Now, duedate) >= 0 ? new SolidColorBrush(Colors.Red) : result >= 75 ? new SolidColorBrush(Colors.Orange) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2e3137"));
            //cheking if the current user is the assigned user for this task.
            BorderColor = Email == EmailAssignee ? new SolidColorBrush(Colors.Blue) : null;
        }

        public TaskModel(BackendController controller, string email, int id, string title, string description, string emailassignee, DateTime duedate, DateTime creationtime) : base(controller)
        {
            this.Id = id;
            this.Email = email;
            this.Title = title;
            this.Descritpion = description;
            this.EmailAssignee = emailassignee;
            this.DueDate = duedate;
            this.CreationTime = creationtime;
            //checking if the task is down or if 75% of its time has passed.
            var total = (DueDate - CreationTime).TotalSeconds;
            var percentage = (DateTime.Now - CreationTime).TotalSeconds * 100 / total;
            int result = (int)percentage;
            _TaskBackground = DateTime.Compare(DateTime.Now, duedate) >= 0 ? new SolidColorBrush(Colors.Red) : result >= 075 ? new SolidColorBrush(Colors.Orange) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2e3137"));
            //cheking if the current user is the assigned user for this task.
            BorderColor = Email == EmailAssignee ? new SolidColorBrush(Colors.Blue) : new SolidColorBrush(Colors.Black);

        }

        //================================================================================================================================================================================

        public void Update(DateTime dueDate, string title, string descritpion)
        {
            this.DueDate = dueDate;
            this.Title = title;
            this.Descritpion = descritpion;
        }

        //================================================================================================================================================================================
        //Bindings.

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged("Id");
            }
        }

        private string _Title;
        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _Descritpion;
        public string Descritpion
        {
            get => _Descritpion;
            set
            {
                _Descritpion = value;
                RaisePropertyChanged("Descritpion");
            }
        }

        private string _emailassignee;
        public string EmailAssignee
        {
            get => _emailassignee;
            set
            {
                _emailassignee = value;
                RaisePropertyChanged("EmailAssignee");
            }
        }

        private DateTime _DueDate;
        public DateTime DueDate
        {
            get => _DueDate;
            set
            {
                _DueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        private DateTime _CreationTime;
        public DateTime CreationTime
        {
            get => _CreationTime;
            set
            {
                _CreationTime = value;
                RaisePropertyChanged("CreationTime");
            }
        }

        private bool _IsVisible = true;
        public bool IsVisible
        {
            get => _IsVisible;
            set
            {
                _IsVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }

        private SolidColorBrush _TaskBackground;
        public SolidColorBrush TaskBackground
        {
            get
            {
                return _TaskBackground;
            }
            set
            {
                //checking if the task is down or if 75% of its time has passed.
                var total = (DueDate - CreationTime).TotalSeconds;
                var percentage = (DateTime.Now - CreationTime).TotalSeconds * 100 / total;
                int result = (int)percentage;
                _TaskBackground = DateTime.Compare(DateTime.Now, DueDate) >= 0 ? new SolidColorBrush(Colors.Red) : result >= 75 ? new SolidColorBrush(Colors.Orange) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2e3137"));
                RaisePropertyChanged("TaskBackground");
            }
        }

        private SolidColorBrush _BorderColor;
        public SolidColorBrush BorderColor
        {
            get
            {
                return _BorderColor;
            }
            set
            {
                //cheking if the current user is the assigned user for this task.
                _BorderColor = Email == EmailAssignee ? new SolidColorBrush(Colors.Blue) : null;
                RaisePropertyChanged("BorderColor");
            }
        }
    }
}
