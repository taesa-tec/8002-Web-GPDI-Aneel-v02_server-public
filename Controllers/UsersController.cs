﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;
using APIGestor.Security;
using System.IdentityModel.Tokens.Jwt;
using System;
using APIGestor.Dtos;
using AutoMapper;
using Microsoft.Extensions.FileProviders;

namespace APIGestor.Controllers
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
        public ActionResult<ApplicationUserDto> Get(string id)
        {
            var User = _service.Obter(id);
            if (User != null)
                return mapper.Map<ApplicationUserDto>(User);
            else
                return NotFound();
        }

        [AllowAnonymous]
        [HttpGet("{id}/avatar")]
        [ResponseCache(Duration = 60)]
        public FileResult Download(string id)
        {
            byte[] image;
            var user = _service.Obter(id);

            if (user == null || user.FotoPerfil == null || user.FotoPerfil.File.Length < 1)
            {
                image = System.IO.File.ReadAllBytes("wwwroot/Assets/default_avatar.jpg");
            }
            else
            {
                image = user.FotoPerfil.File;
            }

            return File(image, System.Net.Mime.MediaTypeNames.Image.Jpeg);
        }

        [HttpGet("me")]
        public ActionResult<ApplicationUserDto> GetA()
        {
            return mapper.Map<ApplicationUserDto>(_service.Obter(this.userId()));
        }

        [HttpPost]
        public ActionResult<Resultado> Post([FromBody] ApplicationUser User)
        {
            if (this.isAdmin())
                return _service.Incluir(User);
            return Forbid();
        }

        [HttpPut]
        public ActionResult<Resultado> Put([FromBody] ApplicationUser User)
        {
            if (this.isAdmin())
                return _service.Atualizar(User);
            return Forbid();
        }

        [HttpPut("me")]
        public ActionResult<Resultado> PutA([FromBody] ApplicationUser _user)
        {
            var me = _service.Obter(this.userId());
            _user.Id = this.userId();
            _user.Email = me.Email;
            _user.Role = me.Role;
            return _service.Atualizar(_user);
        }

        [HttpDelete("{id}")]
        public ActionResult<Resultado> Delete(string id)
        {
            if (this.isAdmin())
                return _service.Excluir(id);
            return Forbid();
        }
    }
}