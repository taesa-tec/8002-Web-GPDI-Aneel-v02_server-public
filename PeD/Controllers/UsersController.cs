using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using PeD.Core.ApiModels;
using PeD.Core.Extensions;
using PeD.Core.Models;
using PeD.Core.Requests.Users;
using PeD.Services;

namespace PeD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class UsersController : ControllerBase
    {
        private UserService _service;
        private IMapper mapper;

        public UsersController(UserService service, IMapper mapper)
        {
            _service = service;
            this.mapper = mapper;
        }

        [HttpGet]
        public IEnumerable<ApplicationUserDto> Get()
        {
            //return _service.ListarTodos();
            return mapper.Map<IEnumerable<ApplicationUserDto>>(_service.ListarTodos());
        }

        [HttpGet("{id}")]
        public ActionResult<ApplicationUserDto> Get(string id, [FromServices] UserManager<ApplicationUser> userManager)
        {
            var User = _service.Obter(id);

            if (User != null)
            {
                User.Roles = userManager.GetRolesAsync(User).Result.ToList();
                return mapper.Map<ApplicationUserDto>(User);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult<Resultado> Post([FromBody] NewUserRequest user)
        {
            if (this.IsAdmin())
                return _service.Incluir(mapper.Map<ApplicationUser>(user));
            return Forbid();
        }

        [HttpPost("{userId}/Avatar")]
        [RequestSizeLimit(5242880)] // 5MB
        public async Task<IActionResult> UploadAvatar(IFormFile file, [FromRoute] string userId,
            [FromServices] IConfiguration configuration)
        {
            await _service.UpdateAvatar(userId, file);
            return Ok();
        }

        [HttpPut]
        public ActionResult<Resultado> Edit([FromBody] ApplicationUser User)
        {
            if (this.IsAdmin())
                return _service.Atualizar(User);
            return Forbid();
        }


        [HttpDelete("{id}")]
        public ActionResult<Resultado> Delete(string id)
        {
            if (this.IsAdmin())
                return _service.Excluir(id);
            return Forbid();
        }

        [HttpGet("Role/{role}")]
        public ActionResult<List<ApplicationUserDto>> GetByRole(string role,
            [FromServices] UserManager<ApplicationUser> userManager)
        {
            var users = userManager.GetUsersInRoleAsync(role).Result.ToList();
            return mapper.Map<List<ApplicationUserDto>>(users);
        }
    }
}