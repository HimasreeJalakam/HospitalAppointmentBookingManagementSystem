using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Models.Data;

public class DB
{
        private SqlConnection connection;

        public DB(AppDbContext context)
        {
            connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString);
        }


        #region Connection
        public SqlConnection GetConnection()
        {
            connection.Open();
            return connection;
        }
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

        }
        #endregion
        public DataTable FillAndReturnDataTable(string SelectQuery)
        {

            SqlDataAdapter adp = new SqlDataAdapter(SelectQuery, GetConnection());
            DataTable dt = new DataTable();
            adp.Fill(dt);
            CloseConnection();
            return dt;

        }
}
