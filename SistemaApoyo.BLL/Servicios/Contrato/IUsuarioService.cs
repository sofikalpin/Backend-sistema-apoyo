using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaApoyo.DTO;



namespace SistemaApoyo.BLL.Servicios.Contrato
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> Lista();
        Task<UsuarioDTO?> ValidarCredencialesAsync(string correo, string contraseña);
        Task<UsuarioDTO> Crear(UsuarioDTO modelo);
        Task<bool> Editar(UsuarioDTO modelo);
        string CubreContrasena(string contrasena);

        Task<bool> Eliminar(int id);

        Task<UsuarioDTO> ObtenerUsuarioPorID(int idusuario);
        Task<UsuarioDTO> ObtenerUsuarioPorCorreo(string correo);

        Task<bool> ReestablecerContraseña(string token, string nuevaContraseña);

        Task EnviarCorreoRecuperacion(string correoDestino, string token);
        Task<bool> GenerarTokenRecuperacion(string correo);

        Task<bool> ActualizarCvRuta(int idUsuario, string cvRuta);

    }
}