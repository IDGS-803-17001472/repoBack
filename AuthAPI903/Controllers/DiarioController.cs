﻿using AuthAPI903.Data;
using AuthAPI903.Dtos;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI903.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DiarioController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public DiarioController(UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        AppDbContext context
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            this._context = context;

        }

        [Authorize]
        [HttpGet("paciente/{id}/diarios")]
        public async Task<ActionResult<List<object>>> ObtenerDiariosPorPaciente(int id)
        {
            var diarios = await _context.Entradas
                .Where(e => e.IdPaciente == id)
                .Select(e => new
                {
                    e.Id,
                    e.Contenido,
                    e.Fecha,
                    Paciente = new
                    {
                        e.Paciente.Id,
                        Persona = new
                        {
                            e.Paciente.Persona.Nombre,
                        }
                    },
                    Medicion = new
                    {
                        e.Mediciones
                    }
                })
                .ToListAsync();

            if (diarios == null || !diarios.Any())
            {
                return NotFound("No se encontraron diarios asociados a este paciente.");
            }

            return Ok(diarios);
        }


        [Authorize]
        [HttpGet("diario/{id}")]
        public async Task<ActionResult<object>> ObtenerDiarioPorId(int id)
        {
            // Busca el diario por ID e incluye la emoción asociada
            var diario = await _context.Entradas
                .Where(e => e.Id == id)
                .Include(e => e.Mediciones) // Incluir las mediciones
                .ThenInclude(e => e.Emocion)
                .Select(e => new
                {
                    e.Id,
                    e.Contenido,
                    e.Fecha,
                    Paciente = new
                    {
                        e.Paciente.Id,
                        Persona = new
                        {
                            e.Paciente.Persona.Nombre,
                        }
                    },
                    Mediciones = e.Mediciones.Select(m => new
                    {
                        m.Emocion.EmocionName,
                        m.Nivel
                    }).ToList() // Proyectar las mediciones en el DTO
                })
                .FirstOrDefaultAsync();

            if (diario == null)
            {
                return NotFound("No se encontró el diario con el ID especificado.");
            }

            return Ok(diario);
        }

        [Authorize]
        [HttpGet("emociones-semana/{idPaciente}")]
        public async Task<ActionResult<IEnumerable<EmocionSemanaDto>>> ObtenerEmocionesPorSemana(int idPaciente)
        {
            // Obtén las mediciones de emociones para el paciente específico
            var emocionesPorSemana = await _context.Mediciones
     .FromSqlRaw(
         @"SELECT DATEPART(YEAR, e.Fecha) AS Anio, DATEPART(WEEK, e.Fecha) AS Semana, 
          m.EmocionId, e.PacienteId
          FROM Mediciones m
          INNER JOIN Entradas e ON m.EntradaId = e.Id
          WHERE e.PacienteId = {0}",
         idPaciente)
     .ToListAsync();

            if (emocionesPorSemana == null || !emocionesPorSemana.Any())
            {
                return NotFound("No se encontraron emociones para el paciente en la semana especificada.");
            }

            return Ok(emocionesPorSemana);
        }
    }
}
