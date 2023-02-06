namespace role_management_user.Controllers.Response
{
  public class GetResponse<T>
  {
    public BaseResponse Meta { get; set; }
    public T Data { get; set; }
  }
}