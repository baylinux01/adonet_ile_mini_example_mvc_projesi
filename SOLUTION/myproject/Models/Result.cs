namespace myproject.Models
{
    public class Result
    {
        public bool Success{get; set;}
        public String? Code{get;set;}
        public String? Message{get;set;}

        // public static Result Ok()
        // {
        //     Result result= new Result();
        //     result.Success=true;
        //     result.Code="200";
        //     result.Message="The operation is successful";
        //     return result;
        // }

        // public static Result Fail(String code, String Message)
        // {
        //     Result result= new Result();
        //     result.Success=false;
        //     result.Code=code;
        //     result.Message=Message;
        //     return result;
        // }

    }
}