using AuthAPI903.Data;
using AuthAPI903.Dtos;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthAPI903.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuejaController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public QuejaController(UserManager<AppUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IConfiguration configuration,
                               AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        // API para registrar una nueva queja
        [Authorize(Roles = "profesional")]
        [HttpPost("registrar")]
        public async Task<ActionResult<Queja>> RegisterQueja(Queja queja)
        {
            _context.Quejas.Add(queja);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQuejas), new { id = queja.Id }, queja);
        }

        // API para registrar una nueva queja
        [HttpPost("profesional/registrar")]
        public async Task<ActionResult<Queja>> RegisterProfesionalQueja(Queja queja)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Obtener el ID del usuario loggeado
            var userId = _userManager.GetUserId(User);

            // Buscar el profesional asociado al usuario loggeado
            var profesional = await _context.Profesionales
                                            .FirstOrDefaultAsync(p => p.Usuario.IdAppUser == userId);

            if (profesional == null)
            {
                return BadRequest("El profesional no fue encontrado.");
            }

            // Buscar el profesional asociado al usuario loggeado
            var usuario = await _context.Usuarios
                                            .FirstOrDefaultAsync(p => p.Id == profesional.IdUsuario);

            if (usuario == null)
            {
                return BadRequest("El usuario no fue encontrado.");
            }

            // Buscar el profesional asociado al usuario loggeado
            var persona = await _context.Personas
                                            .FirstOrDefaultAsync(p => p.Id == usuario.IdPersona);

            if (persona == null)
            {
                return BadRequest("La persona no fue encontrado.");
            }

            queja.IdUsuarioNecesita = persona.Id;
            queja.IdUsuarioSolicita = persona.Id;

            _context.Quejas.Add(queja);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQuejas), new { id = queja.Id }, queja);
        }


        [Authorize(Roles = "profesional")]
        [HttpGet("profesional/quejas")]
        public async Task<IActionResult> GetQuejasByProfesional()
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

            // Buscar al usuario asociado al profesional
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == profesional.IdUsuario);

            if (usuario == null)
            {
                return BadRequest("El usuario no fue encontrado.");
            }

            // Buscar la persona asociada al usuario
            var persona = await _context.Personas
                .FirstOrDefaultAsync(p => p.Id == usuario.IdPersona);

            if (persona == null)
            {
                return BadRequest("La persona no fue encontrada.");
            }

            // Obtener las quejas relacionadas con la persona
            var quejas = await _context.Quejas
                .Where(q => q.IdUsuarioNecesita == persona.Id)
                .ToListAsync();

            return Ok(quejas);
        }




        // API para traer todas las quejas
        [HttpGet("todas")]
        public async Task<ActionResult<IEnumerable<Queja>>> GetQuejas()
        {
            return await _context.Quejas.ToListAsync();
        }

        // API para actualizar una queja
        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> UpdateQueja(int id, [FromBody] Queja queja)
        {
            if (id != queja.Id) return BadRequest();

            var quejaExistente = await _context.Quejas.FindAsync(id);
            if (quejaExistente == null) return NotFound();

            quejaExistente.Descripcion = queja.Descripcion;
            quejaExistente.Tipo = queja.Tipo;
            quejaExistente.Estatus = queja.Estatus;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("profesionales")]
        public async Task<ActionResult<List<UserDetailDto>>> GetProfesionalesDetails()
        {
            // Obtener usuarios con datos relacionados
            var users = await _userManager.Users
                .Include(u => u.Usuario)
                .ThenInclude(p => p.Profesional)
                .Include(u => u.Usuario.Persona)
                .AsSplitQuery() // Evitar problemas de concurrencia
                .ToListAsync();

            if (!users.Any())
            {
                return NotFound(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "No users found"
                });
            }

            var userDetails = new List<UserDetailDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // Procesa cada usuario secuencialmente

                userDetails.Add(new UserDetailDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Roles = roles.ToList(),


                    // Validación para evitar referencias nulas en Usuario, Persona, y Profesional
                    Persona = user.Usuario?.Persona == null ? null : new PersonaDto
                    {
                        Id = user.Usuario.Persona.Id,
                        Nombre = user.Usuario.Persona.Nombre,
                        Apellido = $"{user.Usuario.Persona.ApellidoPaterno} {user.Usuario.Persona.ApellidoMaterno}"
                    },
                    Profesional = user.Usuario?.Profesional == null ? null : new ProfesionalDto
                    {
                        Id = user.Usuario.Profesional.Id,
                        Titulo = user.Usuario.Profesional.Titulo
                    }
                });
            }

            return Ok(userDetails);
        }

    }
}
