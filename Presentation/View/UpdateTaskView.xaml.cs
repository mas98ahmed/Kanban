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
    /// Interaction logic for UpdateTaskView.xaml
    /// </summary>
    public partial class UpdateTaskView : Window
    {
        private UpdateTaskViewModel utvm;

        public UpdateTaskView(UserModel u,TaskModel selectedTask)
        {
            InitializeComponent();
            this.utvm = new UpdateTaskViewModel(u, selectedTask);
            this.DataContext = utvm;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
                utvm.Update();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = utvm.Back(); 
            BoardView boardView = new BoardView(user);
            boardView.Show();
            this.Close();
        }
    }
}
