using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using IntroSE.Kanban.Backend.DataAccessLayer.Objects;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controls
{
    class UserControl : DalControl
    {
        private const string UsersTableName = "Users";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public UserControl() : base(UsersTableName) { }
        
        public List<User> SelectAllUsers()
        {
            List<User> result = Select().Cast<User>().ToList();
            return result;
        }

        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            User result = new User(int.Parse(reader.GetValue(0).ToString()), reader.GetString(1), reader.GetString(2), reader.GetString(3), int.Parse(reader.GetValue(4).ToString()) == 1 ? true : false, int.Parse(reader.GetValue(5).ToString()));
            return result;
        }

        public void Insert(User user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UsersTableName} ({DalObject.IDColumnName},{User.Email},{User.Nickname},{User.Password},{User.Host},{User.BoardId}) " +
                        $"VALUES (@idVal,@emailVal,@nicknameVal,@passwordVal,@hostVal,@boardidVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", user.Id);
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.email);
                    SQLiteParameter nicknameParam = new SQLiteParameter(@"nicknameVal", user.nickname);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.password);
                    SQLiteParameter hostParam = new SQLiteParameter(@"hostVal", user.host);
                    SQLiteParameter boardidParam = new SQLiteParameter(@"boardidVal", user.boardid);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(nicknameParam);
                    command.Parameters.Add(passwordParam);
                    command.Parameters.Add(hostParam);
                    command.Parameters.Add(boardidParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();

                    if (res <= 0)
                    {
                        throw new Exception("Nothing changed in the Users Data.");
                    }
                }
                catch
                {
                    //log error
                    log.Error("There is a problem in the user data inserting.");
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
