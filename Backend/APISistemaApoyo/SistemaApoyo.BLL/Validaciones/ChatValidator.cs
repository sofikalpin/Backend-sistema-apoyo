using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SistemaApoyo.DTO;

namespace SistemaApoyo.BLL.Validaciones
{
    public class ChatValidator : AbstractValidator<ChatDTO>
    {
        public ChatValidator() 
        { 
            RuleFor(v => v.FechahoraInicio)
                .NotEmpty().WithMessage("La fecha de inicio es obligatoria.")
                .WithMessage("La hora de inicio debe ser un valor válido dentro del día.");

            RuleFor(v => v.Idusuario1)
            .NotNull().WithMessage("El usuario 1 es obligatorio.");

            RuleFor(v => v.Idusuario2)
            .NotNull().WithMessage("El usuario 1 es obligatorio.");

            RuleFor(v => v.Mensajes)
                .NotEmpty().WithMessage("El chat debe contener al menos un mensaje.")
                .ForEach(mensaje =>
                    mensaje.SetValidator(new MensajeValidator()));
        }
    }
}
