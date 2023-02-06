using System.ComponentModel.DataAnnotations.Schema;

namespace role_management_user.Dtos
{
    public class UserReadDto
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string balance { get; set; }
        public string bank_account { get; set; }
        public string bank_name { get; set; }
        [NotMapped]
        public IEnumerable<MenuReadDto> menus { get; set; }
    }
}