using role_management_user.Controllers.Response;

namespace role_management_user.Model
{
    public class GetAllResponse<T>
    {
        public BaseResponse Meta { get; set; }
        public IEnumerable<T> Data { get; set; }

    }
}