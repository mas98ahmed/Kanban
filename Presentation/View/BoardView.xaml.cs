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
using Presentation.Model;
using Presentation.ViewModel;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        private BoardViewModel bvm;
        private UserModel user;

        public BoardView(UserModel u)
        {
            InitializeComponent();
            this.bvm = new BoardViewModel(u);
            this.DataContext = bvm;
            this.user = u;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            AddTaskView addView = new AddTaskView(user);
            addView.Show();
            this.Close();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.bvm.DeleteTask();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Cannot remove task: " + ex.Message);
            }
        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
                AddColumnView addView = new AddColumnView(user);
                addView.Show();
                this.Close();
        }

        private void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            bool isassigned = bvm.IsAssigned();   
            if (isassigned)
            {
                UpdateTaskView updateView = new UpdateTaskView(user, bvm.SelectedTask);
                updateView.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("You're not assigned to this task.");
            }
        }

        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            bvm.ResetFilter();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            bvm.Filter();
        }

        private void RemoveColumn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bvm.SelectedColumn = (ColumnModel)((Button)sender).DataContext;
                bvm.RemoveColumn();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MoveRight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bvm.SelectedColumn = (ColumnModel)((Button)sender).DataContext;
                bvm.MoveColumnRight();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MoveColumnLeft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bvm.SelectedColumn = (ColumnModel)((Button)sender).DataContext;
                bvm.MoveColumnLeft();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AdvanceTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bvm.AdvanceTask();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SortTasks_Click(object sender, RoutedEventArgs e)
        {
            bvm.SortTasks();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }

        private void AssignTasks_Click(object sender, RoutedEventArgs e)
        {
            AssignTaskView assignView = new AssignTaskView(user, bvm.SelectedTask);
            assignView.Show();
            this.Close();
        }
    }
}
