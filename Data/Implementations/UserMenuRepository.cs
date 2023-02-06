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

        public void updateMultipleUserMenu(User user, List<Menu> menus)
        {
            throw new NotImplementedException();
        }

        public void updateUserMenus(User user, List<Menu> menus)
        {
            var oldItems = context.UserMenus.Where(x => x.user.id == user.id).ToList();
            context.UserMenus.RemoveRange(oldItems);
            foreach(var menu in menus)
            {
                UserMenu userMenu = new UserMenu();
                userMenu.user = user;
                userMenu.menu = menu;
                context.UserMenus.Update(userMenu);
                saveChanges();
            }
        }
    }
}