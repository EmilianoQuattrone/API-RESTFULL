using AutoMapper;
using Data.Context;
using Models.DTOs.User;
using Models.Entities;
using RepositoryPattern.IRepository.Interfaces.IUser;
using Service.IServices;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext applicationDbContext,
                           IUserRepository userRepository,
                           IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public List<UserDto> GetUsers()
        {
            ICollection<User> listUsers = _userRepository.GetUsers();

            List<UserDto> listUsersDtos = listUsers.Select(
                u => _mapper.Map<UserDto>(u)).ToList();

            return listUsersDtos;
        }

        public User GetUser(int id)
        {
            User user = _userRepository.GetUser(id);

            if (user == null)
                throw new InvalidOperationException("No se encontro el usuario.");

            UserDto userDto = _mapper.Map<UserDto>(user);

            return user;
        }
    }
}