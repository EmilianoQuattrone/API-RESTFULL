using Models.DTOs.User;
using Models.Entities;
namespace Service.IServices
{
    public interface IUserService
    {
        List<UserDto> GetUsers();
        User GetUser(int id);
        bool IsUniqueUser(string user);
        Task<UserResponseDto> Login(UserLoginDto userLoginDto);
        Task<User> Register(UserRegisterDto userRegisterDto);
    }
}