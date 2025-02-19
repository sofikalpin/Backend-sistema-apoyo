using SistemaApoyo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.BLL.Servicios.Contrato
{
    public interface IAdministrador
    {
        Task<List<UsuarioDTO>> ListaTotal();
        Task<List<UsuarioDTO>> ListaRol(int numero_rol);

        Task<List<UsuarioDTO>> ConsultaNombre(int rol, string nombre);
        Task<List<UsuarioDTO>> ConsultaNivel(int rol, int nivel);
        Task<UsuarioDTO> ObtenerUsuarioId(int id);

        Task<List<UsuarioDTO>> ListaAutorizacion(bool tipopermiso);
        Task<bool> AutorizarProfesor(int id);

        Task<bool> CrearUsuario(UsuarioDTO usuario);
        Task<bool> ActualizarUsuario(UsuarioDTO usuario);
        Task<bool> EliminarUsuario(int id);
    }
}