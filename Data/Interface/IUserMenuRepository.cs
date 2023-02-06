using role_management_user.Models;

namespace role_management_user.Data.Interface
{
    public interface IUserMenuRepository
    {
        //create user and multiple menu
        void createUserMenus(int[] menuIds, User user);
        //update user and multiple menu
        void updateUserMenus(User user, List<Menu> menus);
        bool saveChanges();

    }
}