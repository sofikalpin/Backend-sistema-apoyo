using FluentValidation;
using SistemaApoyo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.BLL.Validaciones
{
    public class ForoValidator : AbstractValidator<ForoDTO> 
    {
        public ForoValidator() 
        {
            RuleFor(f => f.Nombre)
                .NotEmpty().WithMessage("El foro debe tener un titulo")
                .MaximumLength(50).WithMessage("El titulo del foro puede tener como maximo 50 caracteres");

            RuleFor(f => f.Descripcion)
                .MaximumLength(1000).WithMessage("El puede tener una descripcion con un maximo de 1000 caracteres");

        }
    }
}
