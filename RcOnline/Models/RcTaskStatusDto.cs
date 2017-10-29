using RcOnline.Enum;

namespace RcOnline.Models
{
    public class RcTaskStatusDto
    {
        public int Id {get; set;}
        public int Status {get; set;}
        public int Total {get; set;}
        public int Errors {get; set;}
        public int Success {get; set;}
        public string ErrorMessage {get; set;}

    }
}