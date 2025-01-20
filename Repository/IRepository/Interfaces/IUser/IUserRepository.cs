using Models.DTOs.User;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.IRepository.Interfaces.IUser
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();

        User GetUser(int id);

        bool IsUniqueUser(string user);

        Task<UserResponseDto> Login(UserLoginDto userLoginDto);

        Task<UserDataDto> Register(UserRegisterDto userRegisterDto);
    }
}