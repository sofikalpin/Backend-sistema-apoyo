﻿using FluentValidation;
using SistemaApoyo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SistemaApoyo.BLL.Validaciones
{
    // Validador para el registro de nuevos usuarios
    public class RegistroValidator : AbstractValidator<UsuarioDTO>
    {
        public RegistroValidator()
        {
            RuleFor(u => u.Correo)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El formato del correo no es válido.");

            RuleFor(u => u.ContraseñaHash)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
                .Must(ContenerMayuscula).WithMessage("La contraseña debe contener al menos una letra mayúscula.")
                .Must(ContenerMinuscula).WithMessage("La contraseña debe contener al menos una letra minúscula.")
                .Must(ContenerNumero).WithMessage("La contraseña debe contener al menos un número.")
                .Must(ContenerCaracterEspecial).WithMessage("La contraseña debe contener al menos un carácter especial.");
        }

        private bool ContenerMayuscula(string contrasena)
        {
            return Regex.IsMatch(contrasena, "[A-Z]");
        }

        private bool ContenerMinuscula(string contrasena)
        {
            return Regex.IsMatch(contrasena, "[a-z]");
        }

        private bool ContenerNumero(string contrasena)
        {
            return Regex.IsMatch(contrasena, "[0-9]");
        }

        private bool ContenerCaracterEspecial(string contrasena)
        {
            return Regex.IsMatch(contrasena, "[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]");
        }
    }
}