using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using IntroSE.Kanban.Backend.DataAccessLayer.Objects;

namespace IntroSE.Kanban.Backend.DataAccessLayer.Controls
{
    class ColumnControl : DalControl
    {
        private const string ColumnsTableName = "Columns";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public ColumnControl(): base(ColumnsTableName) { }

        public List<Column> SelectAllColumns()
        {
            List<Column> result = Select().Cast<Column>().ToList();
            return result;
        }

        protected override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            Column result = new Column(int.Parse(reader.GetValue(0).ToString()), reader.GetString(1), int.Parse(reader.GetValue(2).ToString()), int.Parse(reader.GetValue(3).ToString()), int.Parse(reader.GetValue(4).ToString()));
            return result;
        }

        public void Insert(Column column)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnsTableName} ({DalObject.IDColumnName},{Column.Name},\"{Column.Limit}\",{Column.Ordinal},{Column.BoardId}) " +
                        $"VALUES (@idVal,@nameVal,@limitVal,@ordinalVal,@boardidVal);";
                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", column.Id);
                    SQLiteParameter nameParam = new SQLiteParameter(@"nameVal", column.name);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limitVal", column.limit);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"ordinalVal", column.ordinal);
                    SQLiteParameter boardidParam = new SQLiteParameter(@"boardidVal", column.boardid);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(limitParam);
                    command.Parameters.Add(ordinalParam);
                    command.Parameters.Add(boardidParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();

                    if (res <= 0)
                    {
                        throw new Exception("Nothing changed in the Columns Data.");
                    }
                }
                catch
                {
                    //log error
                    log.Error("There is a problem in the column data inserting.");
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
