﻿using System.Collections.Generic;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class CategoriaContabil : BaseEntity
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
        public List<CategoriaContabilAtividade> Atividades { get; set; }
    }

    public class CategoriaContabilValidator : AbstractValidator<CategoriaContabil>
    {
        public CategoriaContabilValidator()
        {
            RuleFor(r => r.Nome).NotEmpty();
            RuleFor(r => r.Valor).NotEmpty();
        }
    }
}