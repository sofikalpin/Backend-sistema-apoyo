using FluentValidation;
using SistemaApoyo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.BLL.Validaciones
{
    public class UsuarioValidator : AbstractValidator<UsuarioDTO>
    {
        public UsuarioValidator() 
        {
            RuleFor(u => u.Nombrecompleto)
                .NotEmpty().WithMessage("Se debe tener un nombre para el usuario")
                .MaximumLength(50).WithMessage("El nombre tiene como maximo 50 caracteres");

            RuleFor(u => u.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El formato del correo no es válido.");

            RuleFor(u => u.ContraseñaHash)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            RuleFor(u => u.Idnivel)
                .NotNull().WithMessage("El id del nivel es obligatorio.");

            RuleFor(u => u.Idrol)
                .NotNull().WithMessage("El id del rol es obligatorio.");
        }
    }
}
