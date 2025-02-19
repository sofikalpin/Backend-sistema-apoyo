using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DAL.Repositorios.Contrato;
using SistemaApoyo.DTO;
using SistemaApoyo.Model;
using SistemaApoyo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SistemaApoyo.BLL.Servicios
{
    public class AdministradorService : IAdministrador
    {
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<object> _passwordHasher;

        public AdministradorService(IGenericRepository<Usuario> usuarioRepositorio, IMapper mapper)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
            _passwordHasher = new PasswordHasher<object>();
        }

        public async Task<List<UsuarioDTO>> ListaTotal()
        {
            try
            {
                var consulta = await _usuarioRepositorio.Consultar();
                var listaUsuarios = await consulta.ToListAsync();
                return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los usuarios.", ex);
            }
        }

        public async Task<List<UsuarioDTO>> ListaRol(int numero_rol)
        {
            try
            {
                var Usuarioquery = await _usuarioRepositorio.Consultar();
                if (numero_rol >= 1 && numero_rol <= 3)
                {
                    Usuarioquery = Usuarioquery.Where(u => u.Idrol == numero_rol);
                }

                var listaresultado = await Usuarioquery.ToListAsync();
                return _mapper.Map<List<UsuarioDTO>>(listaresultado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los usuarios por rol.", ex);
            }
        }

        public async Task<List<UsuarioDTO>> ConsultaNombre(int rol, string nombre)
        {
            try
            {
                var usuarioquery = await _usuarioRepositorio.Consultar();
                if (!string.IsNullOrEmpty(nombre) && rol >= 1 && rol <= 3)
                {
                    usuarioquery = usuarioquery
                    .Where(u => u.Idrol == rol && EF.Functions.Like(u.Nombrecompleto, $"%{nombre}%"));
                }

                var listaResultado = await usuarioquery.ToListAsync();

                return _mapper.Map<List<UsuarioDTO>>(listaResultado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la actividad por nombre y id.", ex);
            }
        }

        public async Task<List<UsuarioDTO>> ConsultaNivel(int rol, int nivel)
        {
            try
            {
                var Usuarioquery = await _usuarioRepositorio.Consultar();
                if (rol != null && rol >= 1 && rol <= 3 && nivel != null)
                {
                    Usuarioquery = Usuarioquery.Where(u => u.Idrol == rol && u.Idnivel == nivel);
                }

                var listaresultado = await Usuarioquery.ToListAsync();
                return _mapper.Map<List<UsuarioDTO>>(listaresultado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la actividad por nivel y id.", ex);
            }
        }

        public async Task<UsuarioDTO> ObtenerUsuarioId(int id)
        {
            try
            {
                var usuario = await _usuarioRepositorio.Obtener(u => u.Idusuario == id);
                if (usuario == null)
                {
                    throw new InvalidOperationException("Usuario no encontrada.");
                }
                return _mapper.Map<UsuarioDTO>(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la usuario por id.", ex);
            }

        }

        public async Task<bool> CrearUsuario(UsuarioDTO usuario)
        {
            try
            {
                var usuarioCreado = await _usuarioRepositorio.Crear(_mapper.Map<Usuario>(usuario));
                if (usuarioCreado.Idusuario == 0)
                    throw new InvalidOperationException("El ID del usuario es cero, lo cual no es válido.");

                var query = await _usuarioRepositorio.Consultar(u => u.Idusuario == usuarioCreado.Idusuario);
                usuarioCreado = query.Include(rol => rol.IdrolNavigation).First();
                usuario.ContraseñaHash = HashearContrasena(usuario.ContraseñaHash);

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el usuario.", ex);
            }
        }

        public async Task<bool> ActualizarUsuario(UsuarioDTO usuario)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(usuario);
                var usuarioEncontrado = await _usuarioRepositorio.Obtener(a => a.Idusuario == usuarioModelo.Idusuario);
                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                if (!string.IsNullOrEmpty(usuarioModelo.ContraseñaHash))
                {
                    usuarioEncontrado.ContraseñaHash = HashearContrasena(usuarioModelo.ContraseñaHash);
                }

                if (usuario.Idusuario <= 0 || usuario.Idnivel <= 0)
                {
                    throw new ArgumentException("Datos inválidos: asegúrate de que el ID de usuario y el nivel sean válidos.");
                }

                usuarioEncontrado.Nombrecompleto = usuarioModelo.Nombrecompleto;
                usuarioEncontrado.Correo = usuarioModelo.Correo;
                usuarioEncontrado.Idnivel = usuarioModelo.Idnivel;

                if (usuarioEncontrado.Idrol == 1)
                {
                    usuarioEncontrado.AutProf = true;
                }
                else
                {
                    usuarioEncontrado.AutProf = null;
                }

                _mapper.Map(usuario, usuarioEncontrado);
                await _usuarioRepositorio.Editar(usuarioEncontrado);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario.", ex);
            }
        }

        public async Task<bool> EliminarUsuario(int id)
        {
            try
            {
                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.Idusuario == id);
                if (usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no fue encontrado.");

                bool respuesta = await _usuarioRepositorio.Eliminar(usuarioEncontrado);
                if (!respuesta)
                    throw new TaskCanceledException("No se pudo eliminar");
                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<UsuarioDTO>> ListaAutorizacion(bool tipopermiso)
        {
            try
            {
                var Usuarioquery = await _usuarioRepositorio.Consultar();
                Usuarioquery = Usuarioquery.Where(u => (u.AutProf as bool?) == tipopermiso);
                var listaResultado = await Usuarioquery.ToListAsync();
                return _mapper.Map<List<UsuarioDTO>>(listaResultado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los usuarios por si estan autorizados o no.", ex);
            }
        }


        public async Task<bool> AutorizarProfesor(int id)
        {
            try
            {
                var usuarioBuscado = await _usuarioRepositorio.Obtener(u => u.Idusuario == id);

                if (usuarioBuscado == null)
                {
                    throw new TaskCanceledException("El profesor no existe");
                }

                usuarioBuscado.AutProf = true;
                await _usuarioRepositorio.Editar(usuarioBuscado);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public string HashearContrasena(string contrasena)
        {
            if (string.IsNullOrEmpty(contrasena))
            {
                throw new ArgumentException("La contraseña no puede estar vacía.");
            }
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var iteraciones = 10000;
            using (var pbkdf2 = new Rfc2898DeriveBytes(contrasena, salt, iteraciones, HashAlgorithmName.SHA256))
            {
                var hash = pbkdf2.GetBytes(32);

                var resultado = new byte[salt.Length + hash.Length];
                Array.Copy(salt, 0, resultado, 0, salt.Length);
                Array.Copy(hash, 0, resultado, salt.Length, hash.Length);

                return Convert.ToBase64String(resultado);
            }
        }

        public bool VerificarContrasena(string contrasena, string hashAlmacenado)
        {
            var datosHash = Convert.FromBase64String(hashAlmacenado);

            var salt = new byte[16];
            Array.Copy(datosHash, 0, salt, 0, 16);

            var hashOriginal = new byte[32];
            Array.Copy(datosHash, 16, hashOriginal, 0, 32);

            var iteraciones = 10000;
            using (var pbkdf2 = new Rfc2898DeriveBytes(contrasena, salt, iteraciones, HashAlgorithmName.SHA256))
            {
                var hashRecalculado = pbkdf2.GetBytes(32);

                Console.WriteLine($"Salt: {Convert.ToBase64String(salt)}");
                Console.WriteLine($"Hash recalculado: {Convert.ToBase64String(hashRecalculado)}");
                Console.WriteLine($"Hash original: {Convert.ToBase64String(hashOriginal)}");

                return hashOriginal.SequenceEqual(hashRecalculado);

            }
        }
    }
}
