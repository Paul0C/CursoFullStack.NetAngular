using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CursoWebApi.API.Extensions;
using CursoWebApi.Application.Contratos;
using CursoWebApi.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CursoWebApi.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService,
                              ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet("GetUser")]   
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();

                var user = await _userService.GetUserByUserNameAsync(userName);
                return Ok(userName);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar recuperar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]   
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if(await _userService.UserExists(userDto.UserName)) return BadRequest("Usuário já existe");
                    
                var user = await _userService.CreateAccountAsync(userDto);
                
                if(user != null){
                    return Ok(new{
                        userName = user.Username,
                        PrimeiroNome = user.PrimeiroNome,
                        token = _tokenService.CreateToken(user).Result
                    });
                }

                return BadRequest("Usuário não criado, tente novamente mais tarde!");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar Registrar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]   
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await _userService.GetUserByUserNameAsync(userLogin.Username);
                if(user == null) return Unauthorized("Usuário ou senha estão incorretos.");

                var result = await _userService.CheckUserPasswordAsync(user, user.PassWord);
                if(!result.Succeeded) return Unauthorized();

                return Ok(new 
                {
                    userName = user.Username,
                    PrimeiroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar realizar Login. Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]   
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userService.GetUserByUserNameAsync(User.GetUserName());
                if(user == null) return Unauthorized("Usuário inválido.");

                var userReturn = await _userService.UpdateAccount(userUpdateDto);
                if(user == null) return NoContent();
                    
                return Ok(userReturn);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar Atualizar Usuário. Erro: {ex.Message}");
            }
        }
    }
}