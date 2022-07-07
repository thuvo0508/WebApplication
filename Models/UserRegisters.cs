using System.ComponentModel.DataAnnotations;
namespace WebApplication.Models
{
    public class UserRegisters
    {
        [Required]
        public string UserName
        {
            get;
            set;
        }
        [Required]
        public string Password
        {
            get;
            set;
        }
        public string EmailId
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public UserRegisters() { }
    }
}