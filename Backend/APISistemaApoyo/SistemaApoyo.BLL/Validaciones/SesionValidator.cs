using FluentValidation;
using SistemaApoyo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.BLL.Validaciones
{
    public class SesionValidator : AbstractValidator<SesionDTO>
    {
        public SesionValidator() 
        {
            RuleFor(s => s.NombreCompleto)
                .NotEmpty().WithMessage("La sesion debe tener un nombre se usuario")
                .MaximumLength(50).WithMessage("El nombre tiene como maximo 50 caracteres");

            RuleFor(s => s.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El formato del correo no es válido.");

            RuleFor(s => s.Rol)
                .NotNull().WithMessage("El rol no debe estar vacio");
        }
    }
}
