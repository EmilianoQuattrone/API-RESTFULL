using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.User;
using Models.Entities;
using Models.Response;
using Service.IServices;

namespace Controller.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        protected ResponseAPI _responseAPI;

        public UsersController(IUserService userService)
        {
            _userService = userService;
            this._responseAPI = new ResponseAPI();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsers()
        {
            try
            {
                List<UserDto> users = _userService.GetUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("{userId:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUser(int userId)
        {
            try
            {
                User user = _userService.GetUser(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            bool validateNameUser = _userService.IsUniqueUser(userRegisterDto.NameUser);

            if (!validateNameUser)
            {
                _responseAPI.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responseAPI.IsSucces = false;
                _responseAPI.ErrorMesages.Add("El nombre de usuario ya existe.");
                return BadRequest(_responseAPI);
            }

            Task<User> user = _userService.Register(userRegisterDto);
            if (user == null)
            {
                _responseAPI.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responseAPI.IsSucces = false;
                _responseAPI.ErrorMesages.Add("Error en el registro.");
                return BadRequest(_responseAPI);
            }

            _responseAPI.StatusCode = System.Net.HttpStatusCode.OK;
            _responseAPI.IsSucces = true;
            return Ok(_responseAPI);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            UserResponseDto respuestaLogin = await _userService.Login(userLoginDto);
            
            if (respuestaLogin.User == null || string.IsNullOrEmpty(respuestaLogin.Token))
            {
                _responseAPI.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _responseAPI.IsSucces = false;
                _responseAPI.ErrorMesages.Add("El nombre de usuario o password son incorrectos.");
                return BadRequest(_responseAPI);
            }

            _responseAPI.StatusCode = System.Net.HttpStatusCode.OK;
            _responseAPI.IsSucces = true;
            _responseAPI.Result = respuestaLogin;
            return Ok(_responseAPI);
        }
    }
}