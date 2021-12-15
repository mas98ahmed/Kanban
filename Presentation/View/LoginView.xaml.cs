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
using Presentation.ViewModel;
using Presentation.Model;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private LoginViewModel lvm;

        public LoginView()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
            this.lvm = (LoginViewModel)DataContext;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = lvm.Login();
            if (u != null)
            {
                BoardView boardView = new BoardView(u);
                boardView.Show();
                this.Close();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
           lvm.Register();
        }
    }
}
