using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using role_management_user.Controllers.Response;
using role_management_user.Data.Interface;
using role_management_user.Dtos;
using role_management_user.Model;
using role_management_user.Models;

namespace role_management_user.Controllers
{
    [Authorize(Roles = "superadmin")]
    [ApiController]
    [Route("api/menu")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuRepository repository;
        private readonly IMapper mapper;
        private GetResponse<MenuReadDto> getResponse = new GetResponse<MenuReadDto>();
        private GetAllResponse<MenuReadDto> getAllResponse = new GetAllResponse<MenuReadDto>();

        BaseResponse baseResponse = new BaseResponse();

        public MenuController(IMenuRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<MenuReadDto>> menus()
        {
            var menuItems = repository.menus();
            if (menuItems != null)
            {
                baseResponse.code = 200;
                baseResponse.message = "Success Get All Data!";
                getAllResponse.Meta = baseResponse;
                var mappedData = mapper.Map<IEnumerable<MenuReadDto>>(menuItems);
                getAllResponse.Data = mappedData;
                return Ok(getAllResponse);
            }
            baseResponse.code = 404;
            baseResponse.message = "Data Not Found!";
            return NotFound(baseResponse);
        }
        [HttpPost]
        public ActionResult<MenuReadDto> createMenu(MenuCreateDto menuCreateDto)
        {
            if (repository != null)
            {
                var check = repository.checkDuplicatedMenu(menuCreateDto.name);
                if (check)
                {
                    baseResponse.code = 200;
                    baseResponse.message = "Data with name " + menuCreateDto.name + " already exist!";
                    return BadRequest(baseResponse);
                }
                var model = new Menu();
                model.name = menuCreateDto.name;
                repository.createMenu(model);
                repository.saveChanges();
                var mappedData = mapper.Map<MenuReadDto>(model);
                baseResponse.code = 200;
                baseResponse.message = "Success Creating Data!";
                getResponse.Meta = baseResponse;
                getResponse.Data = mappedData;
                return Ok(getResponse);
            }
            else
            {
                baseResponse.code = 400;
                baseResponse.message = "Failed!";
                getResponse.Meta = baseResponse;
                return BadRequest(getResponse);
            }
        }

        [Authorize(Roles = "superadmin")]
        [HttpPut("{id}")]
        public ActionResult updateMenu(int id, MenuUpdateDto menuUpdateDto)
        {
            var modelFromRepo = repository.menuById(id);
            if (modelFromRepo == null)
            {
                baseResponse.code = 404;
                baseResponse.message = "Data Not Found!";
                return NotFound(baseResponse);
            }
            mapper.Map(menuUpdateDto, modelFromRepo);
            repository.updateMenu(modelFromRepo);
            repository.saveChanges();
            baseResponse.code = 200;
            baseResponse.message = "Success Updating Data!";
            return Ok(baseResponse);
        }

        [Authorize(Roles = "superadmin")]
        [HttpDelete("{id}")]
        public ActionResult deleteMenuController(int id)
        {
            var check = repository.menuById(id);
            if(check == null)
            {
                baseResponse.code = 404;
                baseResponse.message = "Data Not Found!";
                return NotFound(baseResponse);
            }
            repository.deleteMenu(id);
            repository.saveChanges();
            baseResponse.code = 200;
            baseResponse.message = "Delete Menu Success!";
            return Ok(baseResponse);
        }
    }
}