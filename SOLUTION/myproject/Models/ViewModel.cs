namespace myproject.Models
{
    public class ViewModel
    {
        public AppUser? AppUser { get; set;}

        public List<AppUser> AppUsers {get;set;}

        public Result? Result{get;set;}

        public List<Result> Results{get;set;}

        public ViewModel()
        {
            this.AppUsers=new List<AppUser>();
            this.Results= new List<Result>();
        }
    }
}