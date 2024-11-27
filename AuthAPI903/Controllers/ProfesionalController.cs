using AuthAPI903.Data;
using AuthAPI903.Dtos;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI903.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesionalController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public ProfesionalController(UserManager<AppUser> userManager,
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
        [HttpGet("paciente/profesionales")]
        public async Task<ActionResult<List<ProfesionalDto>>> ObtenerProfesionalesPorPaciente()
        {
            // Obtener el ID del usuario loggeado
            var userId = _userManager.GetUserId(User);

            // Buscar el paciente asociado al usuario loggeado
            var paciente = await _context.Pacientes
                                         .Include(p => p.Persona)
                                         .FirstOrDefaultAsync(p => p.Persona.Usuario.IdAppUser == userId);

            if (paciente == null)
            {
                return BadRequest("El paciente no fue encontrado.");
            }

            // Obtener las asignaciones de profesionales para el paciente dado
            var asignacionesProfesionales = await _context.AsignacionPacientes
                .Where(ap => ap.IdPaciente == paciente.Id && ap.Profesional.Estatus == true)
                .Include(ap => ap.Profesional.Usuario.Persona) // Incluir la información de Persona del profesional
                .ToListAsync();

            if (asignacionesProfesionales == null || !asignacionesProfesionales.Any())
            {
                return NotFound("No se encontraron profesionales asignados a este paciente.");
            }

            // Construir la lista de profesionales a partir de las asignaciones
            var profesionales = asignacionesProfesionales.Select(ap => new ProfesionalPacienteDto
            {
                IdProfesional = ap.Profesional.Id,
                Titulo = ap.Profesional.Titulo,
                Nombre = ap.Profesional.Usuario.Persona.Nombre,
                ApellidoPaterno = ap.Profesional.Usuario.Persona.ApellidoPaterno,
                ApellidoMaterno = ap.Profesional.Usuario.Persona.ApellidoMaterno,
                Telefono = ap.Profesional.Usuario.Persona.Telefono,
                Email = ap.Profesional.Usuario.Email,
            }).ToList();

            return Ok(profesionales);
        }
    }
}
