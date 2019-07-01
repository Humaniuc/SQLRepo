using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoConnection
{
    public class ConnectionManager
    {
        private static readonly string connString = @"Data Source=DESKTOP-C6SIFK8;Initial Catalog=BookCase;Integrated Security=True";

        private static SqlConnection connection;

        public static SqlConnection GetConnection()
        {
            try
            {
                if(connection == null)
                {
                    connection = new SqlConnection
                    {
                        ConnectionString = connString
                    };
                    connection.Open();
                }
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error when connecting to database: {e.Message}");
                throw;
            }
        }
    }
}
