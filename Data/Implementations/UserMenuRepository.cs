using role_management_user.Data.Interface;
using role_management_user.Models;

namespace role_management_user.Data.Implementations
{
    public class UserMenuRepository : IUserMenuRepository
    {
        private readonly DbContextClass context;

        public UserMenuRepository(DbContextClass context)
        {
            this.context = context;
        }
        public void createUserMenus(int[] menuIds, User user)
        {
            foreach (var menuId in menuIds)
            {
                var menu = context.Menus.FirstOrDefault(x => x.id == menuId);
                if (menu != null)
                {
                    UserMenu userMenu = new UserMenu();
                    userMenu.user = user;
                    userMenu.menu = menu;
                    context.UserMenus.Add(userMenu);
                    saveChanges();
                }
            }
        }

        public bool saveChanges()
        {
            return (context.SaveChanges() >= 0);
        }

        public void updateUserMenus(User user, List<Menu> menus)
        {
            throw new NotImplementedException();
        }
    }
}