using role_management_user.Models;

namespace role_management_user.Data.Interface
{
    public interface IUserRepository
    {
        User authenticate(string username, string password);
        IEnumerable<User> getAllUser();
        User getById(int id);
        User create(User user, string password);
        User userMenuByUserId(int id);

        void update(User user, string password = null);
        void delete(int id);
        bool saveChanges();
    }
}