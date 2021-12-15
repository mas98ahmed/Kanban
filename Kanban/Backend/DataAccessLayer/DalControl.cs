using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    abstract class DalControl
    {
        protected readonly string _connectionString;
        private readonly string _tableName;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public DalControl(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
        }
        public void Update(int id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();

                    if (res <= 0)
                    {
                        throw new Exception("Nothing changed in the Task Data.");
                    }
                }
                catch
                {
                    //log error
                    log.Error("There is a problem in the data updating.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
        }

        public void Update(int id, string attributeName, int attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();

                    if (res <= 0)
                    {
                        throw new Exception("Nothing changed in the Data.");
                    }
                }
                catch
                {
                    //log error
                    log.Error("There is a problem in the data updating.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
        }

        public void Update(long id, string attributeName, DateTime attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={id}"
                };
                try
                {

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue.ToString()));
                    connection.Open();
                    res = command.ExecuteNonQuery();

                    if (res <= 0)
                    {
                        throw new Exception("Nothing changed in the Task Data.");
                    }
                }
                catch
                {
                    //log error
                    log.Error("There is a problem in the data updating.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
        }

        protected List<DalObject> Select()
        {
            List<DalObject> results = new List<DalObject>();
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch
                {
                    //log error
                    log.Error("There is a problem in the data updating.");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        protected abstract DalObject ConvertReaderToObject(SQLiteDataReader reader);

        public void Delete(DalObject DTOObj)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where id={DTOObj.Id}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();

                    if (res <= 0)
                    {
                        throw new Exception("Nothing changed in the Data.");
                    }
                }
                catch
                {
                    //log error
                    log.Error("There is a problem in the data deleting.");
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
