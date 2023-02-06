namespace role_management_user.Models
{
    public class SubMenu : BaseModel
    {
        public string name {get; set;}
        public Menu menu {get; set;}
    }
}