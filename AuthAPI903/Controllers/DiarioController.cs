using AuthAPI903.Data;
using AuthAPI903.Dtos;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

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
        [HttpGet("profesional/ultimos")]
        public async Task<ActionResult<List<object>>> ObtenerUltimosDiariosPorProfesional()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profesional = await _context.Profesionales
                                             .Include(p => p.Usuario)
                                             .FirstOrDefaultAsync(p => p.Usuario.IdAppUser == currentUserId);

            if (profesional == null)
            {
                return NotFound("Profesional no encontrado.");
            }

            var ultimosDiarios = await _context.Entradas
                .Where(e => e.Paciente.AsignacionPacientes.Any(a => a.IdProfesional == profesional.Id))
                .OrderByDescending(e => e.Fecha)
                .Take(5)
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

            if (!ultimosDiarios.Any())
            {
                return NotFound("No se encontraron diarios.");
            }

            return Ok(ultimosDiarios);
        }






        [Authorize]
        [HttpPost("sincronizar")]
        public async Task<ActionResult> SincronizarEntradas([FromBody] List<EntradaDto> entradasDto)
        {
            // Obtener el ID del usuario loggeado
            var userId = _userManager.GetUserId(User);

            // Buscar el paciente asociado al usuario loggeado
            var paciente = await _context.Pacientes
                                         .Include(p => p.Persona)
                                         .FirstOrDefaultAsync(p => p.Persona.Usuario.IdAppUser == userId);

            if (paciente == null)
            {
                return BadRequest("Paciente no encontrado.");
            }

            // Obtener todas las entradas existentes del paciente
            var entradasExistentes = _context.Entradas
                                              .Where(e => e.IdPaciente == paciente.Id)
                                              .ToList();

            // Obtener las mediciones asociadas a esas entradas
            var mediciones = _context.Mediciones
                                     .Where(m => entradasExistentes.Select(e => (int?)e.Id).Contains(m.IdEntrada))
                                     .ToList();

            // Eliminar primero las mediciones asociadas
            _context.Mediciones.RemoveRange(mediciones);

            await _context.SaveChangesAsync();

            // Eliminar después las entradas existentes
            _context.Entradas.RemoveRange(entradasExistentes);

            // Agregar las nuevas entradas con sus mediciones
            foreach (var entradaDto in entradasDto)
            {
                var nuevaEntrada = new Entrada
                {
                    Contenido = entradaDto.Contenido,
                    Fecha = entradaDto.Fecha.ToLocalTime(),
                    IdPaciente = paciente.Id,
                    Mediciones = new List<Medicion>
            {
                new Medicion
                {
                    Nivel = entradaDto.NivelEmocion,
                    IdEmocion = entradaDto.IdEmocion
                }
            }
                };

                _context.Entradas.Add(nuevaEntrada);
            }

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Entradas sincronizadas correctamente.");
        }



        [Authorize]
        [HttpGet("paciente/diarios")]
        public async Task<ActionResult<List<EntradaDto>>> ObtenerDiariosPorPaciente()
        {
            // Obtener el ID del usuario loggeado
            var userId = _userManager.GetUserId(User);

            // Buscar el paciente asociado al usuario loggeado
            var paciente = await _context.Pacientes
                                         .Include(p => p.Persona)
                                         .FirstOrDefaultAsync(p => p.Persona.Usuario.IdAppUser == userId);

            if (paciente == null)
            {
                return BadRequest("Paciente no encontrado.");
            }

            // Obtener las entradas con sus mediciones
            var diarios = await _context.Entradas
                .Where(e => e.IdPaciente == paciente.Id)
                .Select(e => new EntradaDto
                {
                    Contenido = e.Contenido,
                    Fecha = e.Fecha,
                    NivelEmocion = e.Mediciones.FirstOrDefault().Nivel,
                    IdEmocion = e.Mediciones.FirstOrDefault().IdEmocion
                })
                .ToListAsync();

            if (diarios == null || !diarios.Any())
            {
                return NotFound("No se encontraron diarios asociados a este paciente.");
            }

            return Ok(diarios);
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
                            e.Paciente.Persona.ApellidoMaterno,
                            e.Paciente.Persona.ApellidoPaterno,
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
