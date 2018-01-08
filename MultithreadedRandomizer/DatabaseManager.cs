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
        string databaseConnectionString;

        OleDbConnection connection = new OleDbConnection();
        public DatabaseManager(string databaseConnectionString)
        {
            this.databaseConnectionString = databaseConnectionString;
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
            connection.ConnectionString = databaseConnectionString;
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
    }
}
