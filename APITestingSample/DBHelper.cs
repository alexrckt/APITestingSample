using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITestingSample
{
    internal class DBHelper
    {
        public DataTable GetQueryResult(String vQuery)
        {
            string uid = TestContext.Parameters["uid"];
            string pwd = TestContext.Parameters["pwd"];

            MySqlConnection connection;
            DataSet ds = new DataSet();
            var connectionString = $"server=sql7.freemysqlhosting.net;uid={uid};pwd={pwd};database=sql7602927";

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                MySqlDataAdapter adp = new MySqlDataAdapter(vQuery, connection);
                adp.Fill(ds);  

                connection.Close();
                connection.Dispose();          
            }
            catch (Exception E)
            {
                Console.WriteLine("Error in getting result of query.");
                Console.WriteLine(E.Message);
                return new DataTable();
            }
            return ds.Tables[0];
        }
    }
}
