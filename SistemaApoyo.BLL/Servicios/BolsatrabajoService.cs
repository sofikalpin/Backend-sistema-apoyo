using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DAL.Repositorios.Contrato;
using SistemaApoyo.DTO;
using SistemaApoyo.Model;

namespace SistemaApoyo.BLL.Servicios
{
    public class BolsatrabajoService : IBolsatrabajoService
    {

        private readonly IGenericRepository<Bolsatrabajo> _bolsaTrabajoRepository;
        private readonly IGenericRepository<Usuario> _usuarioRepository;

        public BolsatrabajoService(
            IGenericRepository<Bolsatrabajo> bolsaTrabajoRepository,
            IGenericRepository<Usuario> usuarioRepository)
        {
            _bolsaTrabajoRepository = bolsaTrabajoRepository;
            _usuarioRepository = usuarioRepository;

        }


        private async Task<BolsatrabajoDTO> MapToDTO(Bolsatrabajo bolsaTrabajo)
        {
            // Recupera el usuario relacionado con el Idusuario
            var usuario = await _usuarioRepository.Obtener(u => u.Idusuario == bolsaTrabajo.Idusuario);

            // Devuelve el DTO con los datos de la entidad BolsaTrabajo
            return new BolsatrabajoDTO
            {
                Idbolsa = bolsaTrabajo.Idbolsa,  // Asignado por la base de datos (autoincremental)
                Idusuario = bolsaTrabajo.Idusuario,
                NombreCompleto = bolsaTrabajo.NombreCompleto,
                Correo = bolsaTrabajo.Correo,
                Profesion = bolsaTrabajo.Profesion,
                Especildad = bolsaTrabajo.Especildad,
                Nivel = bolsaTrabajo.Nivel,
                CvUrl = bolsaTrabajo.CvUrl,
            };
        }


        public async Task<BolsatrabajoDTO?> Create(BolsatrabajoDTO dto)
        {
            // Verifica si el usuario existe
            var usuarioExistente = await _usuarioRepository.Obtener(u => u.Idusuario == dto.Idusuario);
            if (usuarioExistente == null)
            {
                throw new Exception("El usuario no existe.");
            }

            var bolsaTrabajo = new Bolsatrabajo
            {
                // El idbolsa no debe ser asignado explícitamente
                Idusuario = dto.Idusuario,
                NombreCompleto = dto.NombreCompleto,
                Correo = dto.Correo,
                Profesion = dto.Profesion,
                Especildad = dto.Especildad,
                Nivel = dto.Nivel,
                CvUrl = dto.CvUrl
            };

            var createdBolsaTrabajo = await _bolsaTrabajoRepository.Crear(bolsaTrabajo);
            return await MapToDTO(createdBolsaTrabajo);
        }

        public async Task<IEnumerable<BolsatrabajoDTO>> GetAll()
        {
            var queryable = await _bolsaTrabajoRepository.Consultar();
            var bolsaTrabajoList = await queryable.ToListAsync();
            var bolsaTrabajoDTOs = new List<BolsatrabajoDTO>();

            foreach (var bolsaTrabajo in bolsaTrabajoList)
            {
                bolsaTrabajoDTOs.Add(await MapToDTO(bolsaTrabajo));
            }

            return bolsaTrabajoDTOs;
        }

        public async Task<BolsatrabajoDTO?> GetById(int id)
        {
            var bolsaTrabajo = await _bolsaTrabajoRepository.Obtener(b => b.Idbolsa == id);
            return bolsaTrabajo != null ? await MapToDTO(bolsaTrabajo) : null;
        }


        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            return string.Concat(text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
                .Normalize(NormalizationForm.FormC);
        }


        public async Task<IEnumerable<BolsatrabajoDTO>> GetProfesoresIngles()
        {
            var queryable = await _bolsaTrabajoRepository.Consultar();
            var bolsaTrabajoList = await queryable.ToListAsync(); // Carga los datos en memoria

            var bolsaTrabajoDTOs = new List<BolsatrabajoDTO>();

            foreach (var bolsaTrabajo in bolsaTrabajoList)
            {
                // Compara eliminando tildes
                if (RemoveDiacritics(bolsaTrabajo.Especildad.ToLower()) == "ingles")
                {
                    bolsaTrabajoDTOs.Add(await MapToDTO(bolsaTrabajo));
                }
            }

            return bolsaTrabajoDTOs;
        }
        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var actividadEncontrada = await _bolsaTrabajoRepository.Obtener(a => a.Idbolsa == id);
                if (actividadEncontrada == null)
                    throw new TaskCanceledException("JellyJobs no encontrada.");

                bool respuesta = await _bolsaTrabajoRepository.Eliminar(actividadEncontrada);
                if (!respuesta)
                    throw new TaskCanceledException("No se pudo eliminar");
                return respuesta;
            }
            catch
            {
                throw;
            }
        }
    }
}
