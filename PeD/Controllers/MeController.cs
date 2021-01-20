using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels;
using PeD.Core.Extensions;
using PeD.Core.Models;
using PeD.Services;

namespace PeD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class MeController : ControllerBase
    {
        private UserService _service;
        private IMapper mapper;

        public MeController(IMapper mapper, UserService service)
        {
            this.mapper = mapper;
            _service = service;
        }

        [HttpGet("")]
        public ActionResult<ApplicationUserDto> GetMe()
        {
            return mapper.Map<ApplicationUserDto>(_service.Obter(this.userId()));
        }

        [HttpPut("")]
        public ActionResult<Resultado> EditMe([FromBody] ApplicationUser _user)
        {
            var me = _service.Obter(this.UserId());
            _user.Id = me.Id;
            _user.Email = me.Email;
            _user.Role = me.Role;
            _user.Status = me.Status;
            return _service.Atualizar(_user);
        }

        [HttpPost("Avatar")]
        [RequestSizeLimit(5242880)] // 5MB
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            await _service.UpdateAvatar(this.UserId(), file);
            return Ok();
        }
    }
}