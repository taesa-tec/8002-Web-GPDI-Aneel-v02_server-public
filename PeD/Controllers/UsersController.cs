using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeDetective;
using PeD.Authorizations;
using PeD.Core.ApiModels;
using PeD.Core.Models;
using PeD.Core.Requests.Users;
using PeD.Services;

namespace PeD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    [Authorize(Policy = Policies.IsUserPeD)]
    public class UsersController : ControllerBase
    {
        private UserService _service;
        private IMapper _mapper;
        private string AvatarPath;
        private IWebHostEnvironment env;
        private ILogger<UsersController> _logger;
        private ContentInspector _contentInspector;

        public UsersController(UserService service, IMapper mapper, IConfiguration configuration,
            IWebHostEnvironment env, ILogger<UsersController> logger, ContentInspector contentInspector)
        {
            _service = service;
            _mapper = mapper;
            this.env = env;
            _logger = logger;
            _contentInspector = contentInspector;
            var storagePath = configuration.GetValue<string>("StoragePath");
            AvatarPath = Path.Combine(storagePath, "avatar");
        }

        [HttpGet]
        public IEnumerable<ApplicationUserDto> Get()
        {
            //return _service.ListarTodos();
            return _mapper.Map<IEnumerable<ApplicationUserDto>>(_service.ListarTodos());
        }


        [HttpGet("{id}")]
        public ActionResult<ApplicationUserDto> Get(string id, [FromServices] UserManager<ApplicationUser> userManager)
        {
            var user = _service.Obter(id);

            if (user != null)
            {
                user.Roles = userManager.GetRolesAsync(user).Result.ToList();
                return _mapper.Map<ApplicationUserDto>(user);
            }

            return NotFound();
        }

        [Authorize(Roles = Roles.Administrador)]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NewUserRequest user)
        {
            try
            {
                await _service.Incluir(_mapper.Map<ApplicationUser>(user));
            }
            catch (Exception e)
            {
                _logger.LogError("Erro ao criar usuário: {Error}", e.Message);
                return Problem("Erro ao criar usuário", statusCode: StatusCodes.Status400BadRequest);
            }

            return Ok();
        }

        [HttpPost("{userId}/Avatar")]
        [RequestSizeLimit(5242880)] // 5MB
        public async Task<IActionResult> UploadAvatar(IFormFile file, [FromRoute] string userId)
        {
            try
            {
                await _service.UpdateAvatar(userId, file);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{userId}/Avatar")]
        public async Task<IActionResult> RemoveAvatar([FromRoute] string userId)
        {
            await _service.UpdateAvatar(userId, null);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("/avatar/{userId}.jpg")]
        public FileResult GetAvatar(string userId)
        {
            var filename = Path.Combine(AvatarPath, $"{userId}.jpg");
            if (!System.IO.File.Exists(filename))
            {
                filename = Path.Combine(env.ContentRootPath, "./wwwroot/Assets/default_avatar.jpg");
            }

            return PhysicalFile(filename, "image/jpg");
        }


        [HttpPut]
        public ActionResult<Resultado> Edit([FromBody] EditUserRequest user)
        {
            if (this.IsAdmin() && user.Id != this.UserId())
            {
                _service.Atualizar(_mapper.Map<ApplicationUser>(user));
                return Ok();
            }

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
            return _mapper.Map<List<ApplicationUserDto>>(users);
        }
    }
}