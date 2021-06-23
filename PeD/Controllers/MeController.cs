using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            return mapper.Map<ApplicationUserDto>(_service.Obter(this.UserId()));
        }

        [HttpPut("")]
        public ActionResult<ApplicationUserDto> EditMe([FromBody] EditMeRequest request)
        {
            var me = _service.Obter(ControllerExtension.UserId(this));
            me.Cargo = request.Cargo;
            me.Cpf = request.Cpf;
            me.NomeCompleto = request.NomeCompleto;
            var resultado = _service.Atualizar(me);
            if (resultado.Sucesso)
                return GetMe();

            return BadRequest(resultado);
        }

        [HttpPost("Avatar")]
        [RequestSizeLimit(5242880)] // 5MB
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            await _service.UpdateAvatar(ControllerExtension.UserId(this), file);
            return Ok();
        }
    }
}