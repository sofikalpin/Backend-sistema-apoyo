using FluentValidation;
using SistemaApoyo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.BLL.Validaciones
{
    public class RespuestaValidator : AbstractValidator<RespuestaDTO>
    {
        public RespuestaValidator() 
        {
            RuleFor(r => r.Contenido)
                .NotEmpty().WithMessage("La respuesta no puede estar vacia")
                .MaximumLength(200000).WithMessage("La respuesta no puede tener mas de 200000 caracteres");

            RuleFor(r => r.Fechahora)
                .NotEmpty().WithMessage("La fecha de la respuesta es obligatoria.")
                .NotEmpty().WithMessage("La hora de la respuesta es obligatoria.");

            RuleFor(r => r.Idusuario)
            .NotNull().WithMessage("El id del usuario es obligatorio.");

            RuleFor(r => r.Idconsulta)
            .NotNull().WithMessage("El id de la consulta es obligatoria.");
        }
    }
}
