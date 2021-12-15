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
    /// Interaction logic for AddTaskView.xaml
    /// </summary>
    public partial class AddTaskView : Window
    {
        private AddTaskViewModel atvm;

        public AddTaskView(UserModel u)
        {
            InitializeComponent();
            this.atvm = new AddTaskViewModel(u);
            this.DataContext = atvm;
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = atvm.Back();
            BoardView boardView = new BoardView(user);
            boardView.Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            atvm.Add();
        }
    }
}
