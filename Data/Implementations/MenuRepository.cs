using role_management_user.Data.Interface;
using role_management_user.Models;

namespace role_management_user.Data.Implementations
{
    public class MenuRepository : IMenuRepository
    {
        private readonly DbContextClass context;

        public MenuRepository(DbContextClass context)
        {
            this.context = context;
        }
        public bool checkDuplicatedMenu(string name)
        {
            var check = context.Menus.FirstOrDefault(x => x.name.Equals(name));
            if (check == null)
            {
                return false;
            }
            return true;
        }

        public void createMenu(Menu menu)
        {
            var check = context.Menus.Any(x => x.name == menu.name);
            if (check)
            {
                throw new AppException("Menu name is already taken");
            }
            if (menu == null)
            {
                throw new ArgumentNullException(nameof(menu));
            }
            // if(context.Users.Any(x => x.id == menu.user.id))
            // {
            //     throw new AppException("User Id \"" + menu.user.id + "\" is already taken");
            // }
            context.Menus.Add(menu);
        }

        public void deleteMenu(int id)
        {
            var subMenus = context.SubMenus.Where(x => x.menu.id == id).ToList();
            var menu = context.Menus.Find(id);
            if(menu == null)
            {
                context.SubMenus.RemoveRange(subMenus);
                context.Menus.Remove(menu);
                context.SaveChanges();
            }
        }

        public Menu menuById(int id)
        {
            return context.Menus.FirstOrDefault(x => x.id == id);
        }

        public IEnumerable<Menu> menus()
        {
            var items = context.Menus.ToList();
            foreach(var item in items)
            {
                item.subMenus = context.SubMenus.Where(x => x.menu.id == item.id).ToList();
            }
            return items;
        }

        public bool saveChanges()
        {
            return (context.SaveChanges() >= 0);
        }

        public void updateMenu(Menu menu){}
    }
}