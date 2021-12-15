using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using IntroSE.Kanban.Backend.DataAccessLayer.Objects;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controls
{
    class TaskControl:DalControl
    {
        private const string TasksTableName = "Tasks";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public TaskControl(): base(TasksTableName) { }

        public List<Task> SelectAllTasks()
        {
            List<Task> result = Select().Cast<Task>().ToList();
            return result;
        }

        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            Task result = new Task(int.Parse(reader.GetValue(0).ToString()), reader.GetString(1), DateTime.Parse(reader.GetString(2)), reader.GetString(3),reader.IsDBNull(4)? null: reader.GetString(4), DateTime.Parse(reader.GetString(5)), int.Parse(reader.GetValue(6).ToString()));
            return result;
        }

        public void Insert(Task task)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TasksTableName} ({DalObject.IDColumnName},{Task.EmailAssignee},{Task.CreationTime},{Task.Title},{Task.Description},{Task.DueDate},{Task.ColumnID}) " +
                        $"VALUES (@idVal,@emailVal,@creationtimeVal,@titleVal,@descriptionVal,@duedateVal,@columnidVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", task.Id);
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", task.emailassignee);
                    SQLiteParameter creationtimeParam = new SQLiteParameter(@"creationtimeVal", task.creationtime);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", task.description);
                    SQLiteParameter duedateParam = new SQLiteParameter(@"duedateVal", task.duedate);
                    SQLiteParameter columnidParam = new SQLiteParameter(@"columnidVal", task.columnID);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(creationtimeParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(duedateParam);
                    command.Parameters.Add(columnidParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();

                    if(res <= 0)
                    {
                        throw new Exception("Nothing changed in the Tasks Data.");
                    }
                }
                catch
                {
                    //log error
                    log.Error("There is a problem in the task data inserting.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }
            }
        }
    }
}
