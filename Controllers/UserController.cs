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
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        private readonly IUserMenuRepository userMenuRepository;
        private readonly IMenuRepository menuRepository;
        private readonly IOptions<AppSettings> appSettings;
        private GetResponse<UserReadDto> getResponse = new GetResponse<UserReadDto>();
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

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult getById(int id)
        {
            var check = repository.getById(id);
            if (check == null)
            {
                baseResponse.code = 404;
                baseResponse.message = "User Not Found!";
                return NotFound(baseResponse);
            }
            var user = repository.userMenuByUserId(id);
            var model = mapper.Map<UserReadDto>(user);
            return Ok(model);
        }

        // [Authorize(Roles = "superadmin")]
        // [HttpPut("{id}")]
        // public IActionResult update(int id, [FromForm] UserUpdateDto model, [FromQuery] int[] menuIds)
        // {
        //     List<Menu> menus = new List<Menu>();
        //     var user = repository.getById(id);
 
        //     try
        //     {
        //         foreach(int menuId in menuIds)
        //         {
        //             var menu = menuRepository.menuById(menuId);
        //             if(menu == null)
        //             {
        //                 baseResponse.code = 404;
        //                 baseResponse.message = "Data Not Found!";
        //                 getResponse.Meta = baseResponse;
        //                 return BadRequest(getResponse);
        //             }
        //             menus.Add(menu);
        //         }
        //         mapper.Map(model, user);
        //         repository.update(user, model.password);
        //         userMenuRepository.updateMultipleUserMenu(user, menus);
        //         baseResponse.code = 200;
        //         baseResponse.message = "Update success!";
        //         return Ok(baseResponse);
        //     }
        //     catch (AppException ex)
        //     {
        //         return BadRequest(new { message = ex.Message });
        //     }
        // }

        [Authorize(Roles = "superadmin")]
        [HttpDelete("{id}")]
        public IActionResult delete(int id)
        {
            var check = repository.getById(id);
            if(check == null)
            {
                baseResponse.code = 404;
                baseResponse.message = "User Not Found!";
                return NotFound(baseResponse);
            }
            repository.delete(id);
            baseResponse.code = 200;
            baseResponse.message = "Delete success!";
            return Ok(baseResponse);
        }
    }
}