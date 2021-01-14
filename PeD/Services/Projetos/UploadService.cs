using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PeD.Data;
using PeD.Models.Projetos;

namespace PeD.Services.Projetos
{
    public class UploadService
    {
        private GestorDbContext _context;
        private IWebHostEnvironment _hostingEnvironment;
        public UploadService(GestorDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }
        public IEnumerable<Upload> ObterLogDuto(int projetoId)
        {
            var Upload = _context.Uploads
                .Where(p => p.ProjetoId == projetoId)
                .Where(p => p.Categoria == (CategoriaUpload)4)
                .ToList();
            return Upload;
        }
        public IEnumerable<Upload> ListarTodos(int projetoId)
        {
            var Upload = _context.Uploads
                .Where(p => p.ProjetoId == projetoId)
                .ToList();
            return Upload;
        }
        public Upload Obter(int? id)
        {
            var Upload = _context.Uploads
                .Where(p => p.Id == id)
                .FirstOrDefault();
            return Upload;
        }
        public Resultado Incluir(Upload Upload, IFormCollection Form, string UserId)
        {
            Resultado resultado = DadosValidos(Upload,Form);
            resultado.Acao = "Inclusão de Upload";
            if (resultado.Inconsistencias.Count == 0)
            {
                try
                {
                    string folderName = "uploads/";
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    foreach (var file in Form.Files)
                    {
                        if (file.Length > 0)
                        {
                            string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                            string fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                            var upload = new Upload{
                                NomeArquivo = fileName,
                                UserId = UserId,
                                Url = "wwwroot/"+folderName,
                                ProjetoId = Upload.ProjetoId,
                                TemaId = Upload.TemaId,
                                RegistroFinanceiroId = Upload.RegistroFinanceiroId,
                                RelatorioFinalId = Upload.RelatorioFinalId,
                                ResultadoCapacitacaoId = Upload.ResultadoCapacitacaoId,
                                ResultadoProducaoId = Upload.ResultadoProducaoId,
                                Categoria = Upload.Categoria
                            };   
                            _context.Uploads.Add(upload);
                            _context.SaveChanges();
                            string fullPath = Path.Combine(newPath, upload.Id.ToString());
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                        }
                    }
                    return resultado;
                }
                catch (System.Exception ex)
                {
                    resultado.Inconsistencias.Add(ex.ToString());
                    return resultado;
                }
            }
            return resultado;
            
        }
         private Resultado DadosValidos(Upload Upload, IFormCollection Form)
        {
            var resultado = new Resultado();
            if (Form == null)
            {
                resultado.Inconsistencias.Add("Preencha os Dados do Upload");
            }
            else
            {
                if (Form.Files.ToList().Count() <= 0)
                {
                    resultado.Inconsistencias.Add("Incluia um ou mais arquivos relacionados.");
                }
            }
            return resultado;
        }
        public Resultado Excluir(int id)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Upload";

            var upload = _context.Uploads.FirstOrDefault(t => t.Id == id);
            if (upload == null)
            {
                resultado.Inconsistencias.Add("Upload não localizada");
            }
            else
            {

                _context.Uploads.Remove(upload);
                _context.SaveChanges();
                string fullPath = upload.Url+id;
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            return resultado;
        }
    }
}