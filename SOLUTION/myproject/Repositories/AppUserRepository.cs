using System.Data;
using MySqlConnector;
using myproject.Models;
using Mysqlx.Resultset;
using BCrypt.Net;
using System.Text.RegularExpressions;
namespace myproject.Repositories
{
    public class AppUserRepository
    {
        private MySqlConnection GetCon()
        {
            MySqlConnection con = new MySqlConnection($"server=localhost; database={DatabaseRepository.DbName}; user id=root; password={Environment.GetEnvironmentVariable("MYSQL_PASSWORD")}");
            return con;
        }

        private void CreateAppUserTable()
        {
            MySqlConnection con = null;
            try
            {
                con = GetCon();
                MySqlCommand com = new MySqlCommand($"Create table AppUsers( id bigint primary key auto_increment, name text, surname text, username text unique, password text);", con);
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
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }


        }

        private int InsertIntoAppUsersTable(AppUser appUser)
        {
            int affected = 0;
            MySqlConnection con = null;
            try
            {
                con = GetCon();
                MySqlCommand com = new MySqlCommand($"insert into AppUsers (name,surname,username,password) values (@name,@surname,@username,@password);", con);
                com.Parameters.AddWithValue("@name", appUser.Name);
                com.Parameters.AddWithValue("@surname", appUser.Surname);
                com.Parameters.AddWithValue("@username", appUser.Username);
                com.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.EnhancedHashPassword(appUser.Password,
                Convert.ToInt32(Environment.GetEnvironmentVariable("BCRYPT_DEGREE"))));
                con.Open();
                affected = com.ExecuteNonQuery();
                con.Close();
            }
            catch (System.Exception ex)
            {

                throw new Exception("there was an error in creating AppUserTable"+ex.Message);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return affected;

        }

        private int UpdateAppUser(AppUser appUser)
        {
            int affected = 0;
            if (appUser == null || (appUser.Id<1  && appUser.Username == null))
            {
                return affected;
            }
            // AppUser oldAppUser=null;
            // oldAppUser = GetAppUserById(appUser.Id);
            // if(oldAppUser==null)
            // {
            //     oldAppUser = GetAppUserByUsername(appUser.Username);
            // }
            // if(oldAppUser==null)
            // {
            //     return affected;
            // }  
            MySqlConnection con = null;
            try
            {
                // con = GetCon();
                // MySqlCommand com = new MySqlCommand($"update AppUsers set name=@name,surname=@surname,username=@username,password=@password where id=@id;", con);
                // if (appUser != null && appUser.Name != null)
                // {
                //     com.Parameters.AddWithValue("@name", appUser.Name);
                // }
                // else
                // {
                //     com.Parameters.AddWithValue("@name", oldAppUser.Name);
                // }
                // if (appUser != null && appUser.Surname != null)
                // {
                //     com.Parameters.AddWithValue("@surname", appUser.Surname);
                // }
                // else
                // {
                //     com.Parameters.AddWithValue("@surname", oldAppUser.Surname);
                // }
                // if (appUser != null && appUser.Username != null)
                // {
                //     com.Parameters.AddWithValue("@username", appUser.Username);
                // }
                // else
                // {
                //     com.Parameters.AddWithValue("@username", oldAppUser.Username);
                // }
                // if (appUser != null && appUser.Password != null)
                // {
                //     com.Parameters.AddWithValue("@password", 
                //     BCrypt.Net.BCrypt.EnhancedHashPassword(appUser.Password,
                //     Convert.ToInt32(Environment.GetEnvironmentVariable("BCRYPT_DEGREE"))));
                // }
                // else
                // {
                //     com.Parameters.AddWithValue("@password", 
                //     GetHashedPasswordByUsername(oldAppUser.Username));
                // }
               
                // com.Parameters.AddWithValue("@id", oldAppUser.Id);
                
                con = GetCon();
                MySqlCommand com = new MySqlCommand($"update AppUsers set name=coalesce(@name,name),surname=coalesce(@surname,surname),username=coalesce(@username,username),password=coalesce(@password,password) where id=coalesce(@id,(select id from AppUsers where username=@username limit 1));", con);
                if (appUser != null && appUser.Name != null)
                {
                    com.Parameters.AddWithValue("@name", appUser.Name);
                }
                else
                {
                    com.Parameters.AddWithValue("@name", DBNull.Value);
                }
                if (appUser != null && appUser.Surname != null)
                {
                    com.Parameters.AddWithValue("@surname", appUser.Surname);
                }
                else
                {
                    com.Parameters.AddWithValue("@surname", DBNull.Value);
                }
                if (appUser != null && appUser.Username != null)
                {
                    com.Parameters.AddWithValue("@username", appUser.Username);
                }
                else
                {
                    com.Parameters.AddWithValue("@username", DBNull.Value);
                }
                if (appUser != null && appUser.Password != null)
                {
                    com.Parameters.AddWithValue("@password", 
                    BCrypt.Net.BCrypt.EnhancedHashPassword(appUser.Password,
                    Convert.ToInt32(Environment.GetEnvironmentVariable("BCRYPT_DEGREE"))));
                }
                else
                {
                    com.Parameters.AddWithValue("@password", DBNull.Value);
                }
               
                if(appUser!=null && appUser.Id>0)
                {
                    com.Parameters.AddWithValue("@id", appUser.Id);
                }
                else
                {
                    com.Parameters.AddWithValue("@id", DBNull.Value);
                }

                con.Open();
                affected = com.ExecuteNonQuery();
                con.Close();
            }
            catch (System.Exception ex)
            {

                throw new Exception("there was an error in updating AppUserTable");
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return affected;

        }
        private String GetHashedPasswordByUsername(String username)
        {
            String pass = null;
            MySqlConnection con = null;
            try
            {
                con = GetCon();
                MySqlCommand com = new MySqlCommand($"select password from AppUsers where username=@uname;", con);
                com.Parameters.AddWithValue("@uname", username);

                con.Open();
                MySqlDataReader reader = com.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }
                else
                {
                    pass = reader.GetString("password");
                }
                con.Close();

            }
            catch (System.Exception ex)
            {

                throw new Exception("there was an error in reading a user by username from AppUserTable");
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return pass;
        }
        private bool CheckUsernameInUseByUsername(String username)
        {
            bool exist = true;
            MySqlConnection con = null;
            try
            {
                con = GetCon();
                MySqlCommand com = new MySqlCommand($"select id from AppUsers where username=@uname;", con);
                com.Parameters.AddWithValue("@uname", username);

                con.Open();
                MySqlDataReader reader = com.ExecuteReader();
                if (!reader.Read())
                {
                    exist=false;
                }
                else
                {
                    exist = true;
                }
                con.Close();

            }
            catch (System.Exception ex)
            {

                throw new Exception("there was an error in checking username exists from AppUserTable");
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return exist;
        }
        private bool VerifyLogin(String username, String Password)
        {
            String pass = GetHashedPasswordByUsername(username);
            if (pass != null && BCrypt.Net.BCrypt.EnhancedVerify(Password, pass))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private List<AppUser> GetAllAppUsers()
        {
            List<AppUser> appUsers = new List<AppUser>();
            MySqlConnection con = null;
            try
            {
                con = GetCon();
                MySqlCommand com = new MySqlCommand($"select * from AppUsers;", con);

                con.Open();
                MySqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AppUser u = new AppUser();
                    u.Id=reader.GetInt64("id");
                    u.Name = reader.GetString("name");
                    u.Surname = reader.GetString("surname");
                    u.Username = reader.GetString("username");
                    //u.Password=reader.GetString("password");
                    appUsers.Add(u);
                }
                con.Close();

            }
            catch (System.Exception ex)
            {

                throw new Exception("there was an error in reading users from AppUserTable");
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return appUsers;

        }
        private String LikeEscaper(String input)
        {
            if(input==null||input.Trim().Equals(""))
            {
                return null;
            }
            else
            {
                String input2=Regex.Replace(input,"[^a-zA-Z0-9\\._\\-ÖöÇçİıĞğÜüŞş]","");
                return input2
                            .Replace("\\","\\\\")
                            .Replace("%","\\%")
                            .Replace("_","\\_")
                            .Replace("[","\\[")
                            .Replace("]","\\]")
                            .Replace("^","\\^")
                            .Replace("$","\\$");
            }
        }
        private List<AppUser> SearchAppUsersByPatternOfUsername(String rawSearchExpression)
        {
            String escapedSearchExpression=this.LikeEscaper(rawSearchExpression);
            
            List<AppUser> appUsers = new List<AppUser>();
            MySqlConnection con = null;
            try
            {

                con = GetCon();
                MySqlCommand com = new MySqlCommand($"select * from AppUsers where username like concat('%',@escapedSearchExpression,'%') ESCAPE '\\';", con);
                com.Parameters.AddWithValue("@escapedSearchExpression",escapedSearchExpression);
                con.Open();
                MySqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AppUser u = new AppUser();
                    u.Id=reader.GetInt64("id");
                    u.Name = reader.GetString("name");
                    u.Surname = reader.GetString("surname");
                    u.Username = reader.GetString("username");
                    //u.Password=reader.GetString("password");
                    appUsers.Add(u);
                }
                con.Close();

            }
            catch (System.Exception ex)
            {

                throw new Exception("there was an error in searching users by pattern of username AppUserTable");
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return appUsers;

        }
        private AppUser GetAppUserById(long id)
        {
            List<AppUser> appUsers = new List<AppUser>();
            MySqlConnection con = null;
            try
            {
                con = GetCon();
                MySqlCommand com = new MySqlCommand($"select * from AppUsers where id=@id;", con);
                com.Parameters.AddWithValue("@id", id);

                con.Open();
                MySqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AppUser u = new AppUser();
                    u.Id=reader.GetInt64("id");
                    u.Name = reader.GetString("name");
                    u.Surname = reader.GetString("surname");
                    u.Username = reader.GetString("username");
                    //u.Password=reader.GetString("password");
                    appUsers.Add(u);
                }
                con.Close();

            }
            catch (System.Exception ex)
            {

                throw new Exception("there was an error in reading a user by id from AppUserTable");
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return appUsers[0];

        }

        private AppUser GetAppUserByUsername(String username)
        {
            List<AppUser> appUsers = new List<AppUser>();
            MySqlConnection con = null;
            try
            {
                con = GetCon();
                MySqlCommand com = new MySqlCommand($"select * from AppUsers where username=@uname;", con);
                com.Parameters.AddWithValue("@uname", username);

                con.Open();
                MySqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    AppUser u = new AppUser();
                    u.Id=reader.GetInt64("id");
                    u.Name = reader.GetString("name");
                    u.Surname = reader.GetString("surname");
                    u.Username = reader.GetString("username");
                    //u.Password=reader.GetString("password");
                    appUsers.Add(u);
                }
                con.Close();

            }
            catch (System.Exception ex)
            {

                throw new Exception("there was an error in reading a user by id from AppUserTable");
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return appUsers[0];

        }

        private int DeleteAppUserById(long id)
        {
            int successful = 0;
            MySqlConnection con = null;
            try
            {
                con = GetCon();
                MySqlCommand com = new MySqlCommand($"delete from AppUsers where id=@id;", con);
                com.Parameters.AddWithValue("@id", id);

                con.Open();
                successful = com.ExecuteNonQuery();

                con.Close();

            }
            catch (System.Exception ex)
            {

                throw new Exception("there was an error in deleting a user by id from AppUserTable");
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return successful;

        }

        private int DeleteAppUserByUsername(String username)
        {
            int successful = 0;
            MySqlConnection con = null;
            try
            {
                con = GetCon();
                MySqlCommand com = new MySqlCommand($"delete from AppUsers where username=@uname;", con);
                com.Parameters.AddWithValue("@uname", username);

                con.Open();
                successful = com.ExecuteNonQuery();

                con.Close();

            }
            catch (System.Exception ex)
            {

                throw new Exception("there was an error in deleting a user by id from AppUserTable");
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return successful;

        }

        private int DeleteAppUser(AppUser user)
        {
            String username = user.Username;
            int successful = 0;
            MySqlConnection con = null;
            try
            {
                con = GetCon();
                MySqlCommand com = new MySqlCommand($"delete from AppUsers where username=@uname;", con);
                com.Parameters.AddWithValue("@uname", username);

                con.Open();
                successful = com.ExecuteNonQuery();

                con.Close();

            }
            catch (System.Exception ex)
            {

                throw new Exception("there was an error in deleting a user by id from AppUserTable");
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return successful;

        }

        public int Add(AppUser appUser)
        {
            int affected=this.InsertIntoAppUsersTable(appUser);
            return affected;
        }

        public int Remove(AppUser appUser)
        {
            int result=this.DeleteAppUser(appUser);
            return result;
        }
        public int Update(AppUser appUser)
        {
            int result=this.UpdateAppUser(appUser);
            return result;
        }
        public List<AppUser> FindAll()
        {
            return this.GetAllAppUsers();
        }

        public AppUser FindById(long id)
        {
            return this.GetAppUserById(id);
        }

        public AppUser FindByUsername(String username)
        {
            return this.GetAppUserByUsername(username);
        }

        public AppUser FindAppUser(AppUser user)
        {
            if(user!=null&& user.Id>0)
            {
                return this.FindById(user.Id);
            }
            else if(user!=null&& user.Username!=null&& !user.Username.Trim().Equals(""))
            {
                return this.FindByUsername(user.Username);
            }
            else
            {
                return null;
            }
        }
        public bool CheckLogin(String username,String password)
        {
            return this.VerifyLogin(username,password);
        }
        public bool CheckUsernameInUseOrNot(String username)
        {
            return this.CheckUsernameInUseByUsername(username);
            
        }
        public List<AppUser> SearchAppUsersByUsername(String username)
        {
            return this.SearchAppUsersByPatternOfUsername(username);
        }
    }
}