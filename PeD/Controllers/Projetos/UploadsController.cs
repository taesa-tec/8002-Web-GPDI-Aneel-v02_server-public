using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using PeD.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Models.Projetos;
using PeD.Services.Projetos;

namespace PeD.Controllers.Projetos
{
    [Route("api/upload/")]
    [ApiController]
    [Authorize("Bearer")]
    public class UploadsController : ControllerBase
    {
        private UploadService _service;


        public UploadsController(UploadService service)
        {
            _service = service;
        }
        [HttpGet("{projetoId}/ObterLogDuto")]
        public IEnumerable<Upload> GetA(int projetoId)
        {
            return _service.ObterLogDuto(projetoId);
        }
        [HttpGet("download/{Id}")]
        public FileResult Download(int id)  
        {  
            Upload upload = _service.Obter(id);
            if (upload==null)
                return null;
            byte[] fileBytes = System.IO.File.ReadAllBytes(@upload.Url+id);
            string fileName = upload.NomeArquivo;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost, DisableRequestSizeLimit]
        public ActionResult<Resultado> UploadFile([FromForm]Upload Upload)
        {
            var UserId = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            return _service.Incluir(Upload,Request.Form,UserId);
        }

        [HttpDelete("{Id}")]
        public ActionResult<Resultado> Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}