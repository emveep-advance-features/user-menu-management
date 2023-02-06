using System.ComponentModel.DataAnnotations.Schema;

namespace role_management_user.Models
{
    public class Menu : BaseModel
    {
        public string name {get; set;}
        [NotMapped]
        public IEnumerable<SubMenu> subMenus {get; set;}
    }
}