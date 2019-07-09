using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;
using System.IdentityModel.Tokens.Jwt;
using System.IO;

namespace APIGestor.Controllers
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