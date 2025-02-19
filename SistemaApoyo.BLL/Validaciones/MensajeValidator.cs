using FluentValidation;
using SistemaApoyo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.BLL.Validaciones
{
    public class MensajeValidator : AbstractValidator<MensajeDTO>
    {
        public MensajeValidator() 
        {
            RuleFor(m => m.Contenido)
                .NotEmpty().WithMessage("El mensaje no puede estar vacio")
                .MaximumLength(400).WithMessage("El mensaje puede tener 400 caracteres como maximo");

            RuleFor(m => m.FechaEnvio)
                .NotEmpty().WithMessage("La fecha de inicio es obligatoria.");

            RuleFor(m => m.Idusuario)
            .NotNull().WithMessage("El Id del usuario es obligatorio.");

            RuleFor(m => m.Idusuario)
            .NotNull().WithMessage("El Id del chat es obligatorio.");

        }
    }
}
