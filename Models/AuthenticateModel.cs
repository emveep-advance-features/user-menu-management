using System.ComponentModel.DataAnnotations;

namespace LoginExample.Models
{
    public class AuthenticateModel
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}