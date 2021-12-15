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
    /// Interaction logic for AddColumnView.xaml
    /// </summary>
    public partial class AddColumnView : Window
    {

        private AddColumnViewModel acvm;

        public AddColumnView(UserModel u)
        {
            InitializeComponent();
            this.DataContext = new AddColumnViewModel(u);
            this.acvm = (AddColumnViewModel)DataContext;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = acvm.Back();
            BoardView boardView = new BoardView(user);
            boardView.Show();
            this.Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            acvm.Add();
        }
    }
}
