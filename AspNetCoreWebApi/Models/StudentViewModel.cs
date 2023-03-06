namespace AspNetCoreWebApi.Models
{
    public class StudentViewModel
    {
        public string StudentId { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string NickName { get; set; } = string.Empty;

        public DateTime JoinedAt { get; set; }

        public int SchoolId { get; set; } 

        
    }
}
