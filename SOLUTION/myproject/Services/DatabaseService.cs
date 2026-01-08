using myproject.Repositories;

namespace myproject.Services
{
    public class DatabaseService
    {
        DatabaseRepository dbrepo;
        
        public DatabaseService(DatabaseRepository dbrepo)
        {
            this.dbrepo=dbrepo;
        }

        public void CreateDatabase()
        {
           
            dbrepo.CreateDatabase();
           
        }

        public void CreateAppUsersTable()
        {
           
            dbrepo.CreateAppUsersTable();
            
            
        }
    }
}