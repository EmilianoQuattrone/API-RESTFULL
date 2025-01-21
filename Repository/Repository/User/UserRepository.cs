using Data.Context;
using Models.DTOs.User;
using RepositoryPattern.IRepository.Interfaces.IUser;
using Microsoft.Extensions.Configuration;
using XSystem.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace RepositoryPattern.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly string _secretKey;

        public UserRepository(ApplicationDbContext applicationDbContext,
                              IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _secretKey = configuration["ApiSettings:SecretKey"];
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

        public async Task<UserResponseDto> Login(UserLoginDto userLoginDto)
        {
            string passwordEncrypt = Getmd5(userLoginDto.Password);

            Models.Entities.User user = 
            _applicationDbContext.Users.FirstOrDefault(u => u.NameUser.ToLower() ==
                                                       userLoginDto.NameUser.ToLower()
                                                       && u.Password == passwordEncrypt);

            if(user == null)
            {
                return new UserResponseDto()
                {
                    Token = "",
                    User = null
                };
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.ASCII.GetBytes(_secretKey);

            // Se usa para generar un Token (JWT).
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.NameUser.ToString()),
                    new Claim(ClaimTypes.Role, user.Rol)
                }),

                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new (new SymmetricSecurityKey(key), 
                                          SecurityAlgorithms.HmacSha256Signature)
            };

            
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            UserResponseDto userResponseDto = new UserResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };

            return userResponseDto;
        }

        public async Task<Models.Entities.User> Register(UserRegisterDto userRegisterDto)
        {
            string passwordEncrypt = Getmd5(userRegisterDto.Password);

            Models.Entities.User user = new Models.Entities.User()
            {
                NameUser = userRegisterDto.NameUser,
                Password = passwordEncrypt,
                Name = userRegisterDto.Name,
                Rol = userRegisterDto.Rol,
            };

            _applicationDbContext.Users.Add(user);
            await _applicationDbContext.SaveChangesAsync();
            user.Password = passwordEncrypt;
            return user;
        }

        // Método para encriptar contraseña con MD5 (Visto en el curso).
        private static string Getmd5(string password)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            data = mD5CryptoServiceProvider.ComputeHash(data);
            string response = "";

            for (int i = 0; i < data.Length; i++)
                response += data[i].ToString("x2").ToLower();

            return response;
        }
    }
}