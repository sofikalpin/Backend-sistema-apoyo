using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SistemaApoyo.DTO;


namespace SistemaApoyo.BLL.Validaciones
{
    public class ActividadValidator : AbstractValidator<ActividadDTO>
    {
        public ActividadValidator() 
        {
            RuleFor(a => a.Nombre)
                .NotEmpty().WithMessage("El nombre no debe estar en blanco")
                .MaximumLength(500).WithMessage("El nombre no debe tener mas de 500 caracteres");

            RuleFor(a => a.Descripcion)
                .NotEmpty().WithMessage("La descripcion no debe estar en blanco")
                .Length(5, 20000).WithMessage("La descripcion debe tener como minimo 5 caracteres");

            RuleFor(a => a.Idusuario)
                .GreaterThan(0).When(x => x.Idusuario.HasValue)
                .WithMessage("El ID usuario debe ser un valor positivo.");
            
            RuleFor(a => a.Idnivel)
                .GreaterThan(0).When(x => x.Idnivel.HasValue)
                .WithMessage("El ID nivel debe ser un valor positivo.");

            RuleFor(a => a.FechaCreacion)
                .Must(fecha => fecha == null || fecha <= DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("La fecha no puede ser mayor a la fecha actual");

        }
    }
}
