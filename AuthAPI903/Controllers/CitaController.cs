﻿using AuthAPI903.Data;
using AuthAPI903.Dtos;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthAPI903.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CitaController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public CitaController(UserManager<AppUser> userManager,
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



        [Authorize(Roles = "profesional")]
        [HttpGet("proximas")]
        public async Task<ActionResult<List<CitaDto>>> ObtenerProximasCitasPorProfesional()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profesional = await _context.Profesionales
                                             .Include(p => p.Usuario)
                                             .FirstOrDefaultAsync(p => p.Usuario.IdAppUser == currentUserId);

            if (profesional == null)
            {
                return NotFound("Profesional no encontrado.");
            }

            var proximasCitas = await _context.Citas
                .Where(c => c.AsignacionPaciente.IdProfesional == profesional.Id && c.Fecha > DateTime.Now)
                .OrderBy(c => c.Fecha)
                .Take(5)
                .Select(c => new CitaDto
                {
                    Id = c.Id,
                    Title = "Cita con " + c.AsignacionPaciente.Paciente.Persona.Nombre,
                    Date = c.Fecha,
                    Time = c.Horario,
                    Status = c.Estatus,
                })
                .ToListAsync();

            if (!proximasCitas.Any())
            {
                return NotFound("No hay citas próximas.");
            }

            return Ok(proximasCitas);
        }


        [Authorize(Roles = "profesional")]
        [HttpGet("profesional/{idPaciente}/citas")]
        public async Task<IActionResult> GetCitasDePaciente(int idPaciente)
        {



            // Obtener el ID del usuario loggeado
            var userId = _userManager.GetUserId(User);

            // Buscar el profesional asociado al usuario loggeado
            var profesional = await _context.Profesionales
                                            .FirstOrDefaultAsync(p => p.Usuario.IdAppUser == userId);

            if (profesional == null)
            {
                return BadRequest("El profesional no fue encontrado.");
            }
            // Obtener el ID del profesional autenticado
            var profesionalId = profesional.Id;



            // Obtener las citas del paciente relacionadas con este profesional
            var citas = await _context.Citas
                .Where(c => c.AsignacionPaciente.IdPaciente == idPaciente && c.AsignacionPaciente.IdProfesional == profesionalId)
                .ToListAsync();

            if (citas == null || !citas.Any())
            {
                return NotFound("No se encontraron citas para este paciente con el profesional.");
            }

            return Ok(citas);
        }



        [Authorize]
        [HttpGet("profesional/citas")]
        public async Task<ActionResult<List<CitaDto>>> ObtenerCitasPorProfesional()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profesional = await _context.Profesionales
                                             .Include(p => p.Usuario)
                                             .FirstOrDefaultAsync(p => p.Usuario.IdAppUser == currentUserId);

            if (profesional == null)
            {
                return NotFound("Profesional no encontrado.");
            }

            var citas = await _context.Citas
                .Where(c => c.AsignacionPaciente.IdProfesional == profesional.Id)
                .Select(c => new CitaDto
                {
                    Id = c.Id,
                    Title = "Cita con " + c.AsignacionPaciente.Paciente.Persona.Nombre,
                    Date = c.Fecha,
                    Status = c.Estatus,
                })
                .ToListAsync();

            if (!citas.Any())
            {
                return NotFound("No se encontraron citas.");
            }

            return Ok(citas);
        }

        [Authorize]
        [HttpGet("paciente/citas")]
        public async Task<ActionResult<List<CitaPacienteDto>>> ObtenerCitasPorPaciente()
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

            // Obtener las citas asociadas al paciente
            var citas = await _context.Citas
                .Where(c => c.AsignacionPaciente.IdPaciente == paciente.Id)
                .Select(c => new CitaPacienteDto
                {
                    Id = c.Id,
                    Title = c.Notas ?? "Cita asignada",
                    Date = c.Fecha ?? DateTime.MinValue, // Usa un valor por defecto para fechas nulas
                    Time = c.Horario ?? TimeSpan.Zero,   // Usa un valor por defecto para horas nulas
                    Status = c.Estatus,
                    Profesional = new ProfesionalCitaDto
                    {
                        Nombre = c.AsignacionPaciente.Profesional.Usuario.Persona.Nombre,
                        ApellidoPaterno = c.AsignacionPaciente.Profesional.Usuario.Persona.ApellidoPaterno,
                        ApellidoMaterno = c.AsignacionPaciente.Profesional.Usuario.Persona.ApellidoMaterno,
                        Titulo = c.AsignacionPaciente.Profesional.Titulo,
                    }
                })
                .ToListAsync();

            if (!citas.Any())
            {
                return NotFound("No se encontraron citas para este paciente.");
            }

            return Ok(citas);
        }


        [Authorize]
        [HttpGet("cita/{id}")]
        public async Task<ActionResult<Cita>> ObtenerCitaPorId(int id)
        {
            // Busca la cita por su ID y carga la información del paciente y la persona asociada
            var cita = await _context.Citas
                .Include(c => c.AsignacionPaciente)
                    .ThenInclude(ap => ap.Paciente)
                        .ThenInclude(p => p.Persona)
                        .Select(c => new
                        {
                            c.Id,
                            c.Horario,
                            c.Fecha,
                            c.Estatus,
                            AsignacionPaciente = new
                            {
                                c.AsignacionPaciente.Id,
                                Paciente = new
                                {
                                    Persona = new
                                    {
                                        c.AsignacionPaciente.Paciente.Persona.Nombre,
                                        c.AsignacionPaciente.Paciente.Persona.ApellidoPaterno,
                                        c.AsignacionPaciente.Paciente.Persona.ApellidoMaterno
                                    }
                                }
                            }
                        }).FirstOrDefaultAsync(c => c.Id == id);






            if (cita == null)
            {
                return NotFound("No se encontró la cita con el ID proporcionado.");
            }

            return Ok(cita);
        }

        [Authorize]
        [HttpPut("cita/{id}/reagendar")]
        public async Task<IActionResult> ReagendarCita(int id, [FromBody] ReagendarCitaDto nuevaCita)
        {
            // Busca la cita por su ID
            var cita = await _context.Citas.FirstOrDefaultAsync(c => c.Id == id);

            if (cita == null)
            {
                return NotFound("No se encontró la cita con el ID proporcionado.");
            }

            // Actualiza la fecha y hora de la cita
            cita.Fecha = nuevaCita.Fecha ?? cita.Fecha;
            cita.Horario = nuevaCita.Horario ?? cita.Horario;

            // Marca la cita como modificada
            _context.Citas.Update(cita);

            // Guarda los cambios en la base de datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Ocurrió un problema al actualizar la cita. Intenta nuevamente.");
            }

            return Ok("La cita ha sido reagendada exitosamente.");
        }


        [Authorize]
        [HttpPut("cita/{id}/aceptar")]
        public async Task<IActionResult> AceptarCita(int id)
        {
            // Busca la cita por su ID
            var cita = await _context.Citas.FirstOrDefaultAsync(c => c.Id == id);

            if (cita == null)
            {
                return NotFound("No se encontró la cita con el ID proporcionado.");
            }

            // Verifica si la cita ya ha sido aceptada
            if (cita.Estatus == "Aceptada")
            {
                return BadRequest("La cita ya ha sido aceptada.");
            }

            // Cambia el estado de la cita a 'Aceptada'
            cita.Estatus = "Aceptada";

            // Marca la cita como modificada
            _context.Citas.Update(cita);

            // Guarda los cambios en la base de datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Ocurrió un problema al aceptar la cita. Intenta nuevamente.");
            }

            return Ok("La cita ha sido aceptada exitosamente.");
        }


        [Authorize]
        [HttpPost("registrar-cita")]
        public async Task<ActionResult> RegistrarCita([FromBody] RegistrarCitaDto citaDto)
        {
            var currentUserId = _userManager.GetUserId(User);

            // Buscar el profesional asociado al usuario loggeado
            var profesional = await _context.Profesionales
                                            .FirstOrDefaultAsync(p => p.Usuario.IdAppUser == currentUserId);
            if (profesional == null)
            {
                return BadRequest("El profesional no fue encontrado.");
            }

            // Buscar la asignación de paciente para el profesional dado
            var asignacionPaciente = await _context.AsignacionPacientes
                .FirstOrDefaultAsync(ap => ap.IdPaciente == citaDto.IdPaciente && ap.IdProfesional == profesional.Id);

            if (asignacionPaciente == null)
            {
                return BadRequest("El paciente no está asignado a este profesional.");
            }

            if (!TimeSpan.TryParse(citaDto.Horario, out var horarioTimeSpan))
            {
                return BadRequest("Formato de hora inválido.");
            }

            var cita = new Cita
            {
                IdAsignacion = asignacionPaciente.Id,
                Fecha = citaDto.Fecha,
                Horario = horarioTimeSpan,
                Notas = citaDto.Notas,
                Estatus = "Pendiente"
            };

            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cita registrada exitosamente", citaId = cita.Id });
        }


    }
}
