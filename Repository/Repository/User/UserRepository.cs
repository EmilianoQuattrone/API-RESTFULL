using Data.Context;
using Models.DTOs.User;
using RepositoryPattern.IRepository.Interfaces.IUser;

namespace RepositoryPattern.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public Models.Entities.User GetUser(int id)
        {
            return _applicationDbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public ICollection<Models.Entities.User> GetUsers()
        {
            return _applicationDbContext.Users.OrderBy(u => u.NameUser).ToList();
        }

        public bool IsUniqueUser(string user)
        {
            Models.Entities.User userDB = _applicationDbContext.Users.FirstOrDefault(
                u => u.NameUser == user);

            if (userDB == null)
                return false;

            return true;
        }

        public Task<UserResponseDto> Login(UserLoginDto userLoginDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDataDto> Register(UserRegisterDto userRegisterDto)
        {
            throw new NotImplementedException();
        }
    }
}