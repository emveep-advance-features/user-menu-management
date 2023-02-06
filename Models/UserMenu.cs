namespace role_management_user.Models
{
    public class UserMenu : BaseModel
    {
        public User user {get; set;}
        public Menu menu {get; set;}         
    }
}