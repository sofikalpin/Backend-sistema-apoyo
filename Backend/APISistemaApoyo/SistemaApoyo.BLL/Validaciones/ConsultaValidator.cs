using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SistemaApoyo.DTO;

namespace SistemaApoyo.BLL.Validaciones
{
    public class ConsultaValidator : AbstractValidator<ConsultaDTO>
    {
        public ConsultaValidator() 
        {
            RuleFor(c => c.Contenido)
                .NotEmpty().WithMessage("El mensaje no debe estar vacio")
                .MaximumLength(5000).WithMessage("El mensaje no puede tener maas de 5000 caracteres");

            RuleFor(c => c.Fechahora)
                .NotEmpty().WithMessage("La fecha es obligatoria.")
                .WithMessage("La hora debe ser un valor válido dentro del día.");

            RuleFor(c => c.Titulo)
                .NotEmpty().WithMessage("El titulo del mensaje no debe estar en blanco")
                .MaximumLength(100).WithMessage("El titulo del mensaje no debe tener mas de 30 caracteres");

            RuleFor(c => c.Idusuario)
            .NotNull().WithMessage("El Id usuario es obligatorio.");
        }
    }
}
