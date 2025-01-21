using Models.DTOs.User;
using Models.Entities;
namespace Service.IServices
{
    public interface IUserService
    {
        List<UserDto> GetUsers();
        User GetUser(int id);
    }
}