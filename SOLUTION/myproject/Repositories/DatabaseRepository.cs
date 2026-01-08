using System.Data;
using MySqlConnector;
namespace myproject.Repositories
{
    public class DatabaseRepository
    {
        public static String DbName="DenemeProje";

        public MySqlConnection GetEmptyCon()
        {
            MySqlConnection con=new MySqlConnection($"server=localhost; user id=root; password={Environment.GetEnvironmentVariable("MYSQL_PASSWORD")}");
            return con;
            // MySqlConnection con=new MySqlConnection($"host=localhost; username=root; password={Environment.GetEnvironmentVariable("MYSQL_PASSWORD")}");
            // return con;
        }
        public MySqlConnection GetCon()
        {
            MySqlConnection con=new MySqlConnection($"server=localhost; database={DbName}; user id=root; password={Environment.GetEnvironmentVariable("MYSQL_PASSWORD")}");
            return con;
        }
        public void CreateDatabase()
        {
            MySqlConnection con=null;
            try
            {
                con=GetEmptyCon();
                MySqlCommand com=new MySqlCommand($"Create database {DbName}",con);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (System.Exception ex)
            {
                
                throw new Exception("there was an error in Creating Database");
            }
            finally
            {
                if(con!=null && con.State==ConnectionState.Open)
                {
                    con.Close();
                }
            }
            

        }

        public void CreateAppUsersTable()
        {
            MySqlConnection con=null;
            try
            {
                con=GetCon();
                MySqlCommand com=new MySqlCommand($"Create table AppUsers( id bigint primary key auto_increment, name text, surname text, username text unique, password text);",con);
                con.Open();
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (System.Exception ex)
            {
                
                throw new Exception("there was an error in creating AppUserTable");
            }
            finally
            {
                if(con!=null && con.State==ConnectionState.Open)
                {
                    con.Close();
                }
            }
            

        }
        
    }
}