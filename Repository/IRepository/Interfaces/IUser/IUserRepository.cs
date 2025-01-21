using Models.DTOs.User;
using Models.Entities;

namespace RepositoryPattern.IRepository.Interfaces.IUser
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();

        User GetUser(int id);

        bool IsUniqueUser(string user);

        Task<UserResponseDto> Login(UserLoginDto userLoginDto);

        Task<User> Register(UserRegisterDto userRegisterDto);
    }
}