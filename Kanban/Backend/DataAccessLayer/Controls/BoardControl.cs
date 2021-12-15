using IntroSE.Kanban.Backend.DataAccessLayer.Objects;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controls
{
    class BoardControl : DalControl
    {
        private const string BoardsTableName = "Boards";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public BoardControl() : base(BoardsTableName) { }

        public List<Board> SelectAllBoards()
        {
            List<Board> result = Select().Cast<Board>().ToList();
            return result;
        }

        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            Board result = new Board(int.Parse(reader.GetValue(0).ToString()), reader.GetString(1));
            return result;
        }

        public void Insert(Board board)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardsTableName} ({DalObject.IDColumnName},{Board.EmailHost}) " +
                        $"VALUES (@idVal,@emailhostVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", board.Id);
                    SQLiteParameter emailhostParam = new SQLiteParameter(@"emailhostVal", board.emailhost);
                  
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(emailhostParam);

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
