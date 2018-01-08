using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace MultithreadedRandomizer
{
    class DatabaseManager
    {

        private OleDbConnection connection;

        public DatabaseManager()
        {
            connection = new OleDbConnection();
            connection.ConnectionString = DatabaseInfo.connectionString;
        }

        public Exception checkConnection()
        {
            try
            {
                openConnction();
                closeConnection();
            }
            catch (Exception ex)
            {
                return ex;
            }

            return null;
        }

        public void openConnction()
        {
            
            connection.OpenAsync();
        }

        public void closeConnection()
        {
            connection.Close();
        }

        public void addItem(string commandText)
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = commandText;
            command.ExecuteNonQueryAsync();
        }

        public void clearTable()
        {
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM randomStrings";
            command.ExecuteNonQueryAsync();
        }
    }
}
