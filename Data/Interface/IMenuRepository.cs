using role_management_user.Models;

namespace role_management_user.Data.Interface
{
    public interface IMenuRepository
    {
        IEnumerable<Menu> menus();
        Menu menuById(int id);
        void createMenu(Menu menu);
        void deleteMenu(int id);
        void updateMenu(Menu menu);
        bool saveChanges();
        bool checkDuplicatedMenu(string name);
    }
}