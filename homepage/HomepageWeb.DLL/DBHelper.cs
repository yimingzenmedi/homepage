
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace HomepageWeb.DAL
{
    public class DBHelper: ConfigurationSection
    {
        //static string connectionStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=Homepage;Integrated Security=True";
        static string connectionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString();
        /// <summary>
        /// create a sql connection
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(connectionStr);
            return conn;
        }


        /// <summary>
        /// execute a sql query and return the number of influenced rows
        /// </summary>
        /// <param name="sql">the sql query to be executed</param>
        /// <param name="parameters">parameters of the sql query. If no params needed, pass a null</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sqlQuery, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection()) 
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                {
                    if (parameters != null && parameters.Length > 0)    // in case the parameters is empty 
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// to select from database and return a SqlDataReader
        /// </summary>
        /// <param name="sqlQuery">the sql query to be executed</param>
        /// <param name="parameters">parameters of the sql query. If no params needed, pass a null</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sqlQuery, params SqlParameter[] parameters)
        {
            SqlConnection conn = GetConnection();
            using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
            {

                if (parameters != null && parameters.Length > 0)    // in case the parameters is empty 
                {
                    cmd.Parameters.AddRange(parameters);
                }

                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);  // close connection after executing
            }
        }


        /// <summary>
        /// to select from database and return a datatable
        /// </summary>
        /// <param name="sqlQuery">the sql query to be executed</param>
        /// <param name="parameters">parameters of the sql query. If no params needed, pass a null</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sqlQuery, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqlQuery;
                    if (parameters != null && parameters.Length > 0)    // in case the parameters is empty 
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
            }
        }


        /// <summary>
        /// to select from database and return the first line in the first row
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sqlQuery, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqlQuery;
                    if (parameters != null && parameters.Length > 0)    // in case the parameters is empty 
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}
