using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using LoginExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using role_management_user.Controllers.Response;
using role_management_user.Data;
using role_management_user.Data.Interface;
using role_management_user.Dtos;
using role_management_user.Models;

namespace role_management_user.Controllers
{
    [ApiController]
    [Route("api/menu")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        private readonly IUserMenuRepository userMenuRepository;
        private readonly IMenuRepository menuRepository;
        private readonly IOptions<AppSettings> appSettings;
        BaseResponse baseResponse = new BaseResponse();
        public UserController(IUserRepository repository, IMapper mapper, IUserMenuRepository userMenuRepository, IMenuRepository menuRepository, IOptions<AppSettings> appSettings)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.userMenuRepository = userMenuRepository;
            this.menuRepository = menuRepository;
            this.appSettings = appSettings;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult authenticate([FromBody] AuthenticateModel model)
        {
            var user = repository.authenticate(model.username, model.password);

            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Value.secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString()),
                    new Claim(ClaimTypes.Role, user.role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                id = user.id,
                username = user.username,
                firstName = user.firstName,
                lastName = user.lastName,
                email = user.email,
                role = user.role,
                Token = tokenString
            });
        }
        
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult register([FromQuery] int[] menuIds, [FromForm] RegisterModel model)
        {
            var user = mapper.Map<User>(model);
            try
            {
                foreach (var menuId in menuIds)
                {
                    var check = menuRepository.menuById(menuId);
                    if (check == null)
                    {
                        baseResponse.code = 404;
                        baseResponse.message = "Menu with id : "+ menuId +" is Not Found!";
                        return NotFound(baseResponse);
                    }
                }
                repository.create(user, model.password);
                userMenuRepository.createUserMenus(menuIds, user);
                return Ok(new
                {
                    id = user.id,
                    username = user.username,
                    firstName = user.firstName,
                    lastName = user.lastName,
                    email = user.email,
                    role = user.role
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}