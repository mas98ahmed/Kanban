using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for AssignTaskView.xaml
    /// </summary>
    public partial class AssignTaskView : Window
    {
        private AssignTaskViewModel atvm;

        public AssignTaskView(UserModel user, TaskModel selectedTask)
        {
            InitializeComponent();
            this.atvm = new AssignTaskViewModel(user, selectedTask);
            this.DataContext = atvm;
        }

        private void AssignTask_Click(object sender, RoutedEventArgs e)
        {
            atvm.Assign();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = atvm.Back();
            BoardView boardView = new BoardView(user);
            boardView.Show();
            this.Close();
        }
    }
}
