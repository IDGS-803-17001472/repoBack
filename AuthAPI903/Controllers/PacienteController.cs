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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public PacienteController(UserManager<AppUser> userManager,
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

        [HttpGet("pacientes/{id}")]
        public async Task<ActionResult<PacienteDto>> GetPacienteById(int id)
        {
            var paciente = await _context.Pacientes
                .Include(p => p.Persona) // Incluye la información de Persona
                .Where(p => p.Id == id)
                .Select(p => new PacienteDto
                {
                    IdPaciente = p.Id,
                    Nombre = p.Persona.Nombre,
                    ApellidoMaterno = p.Persona.ApellidoMaterno,
                    ApellidoPaterno = p.Persona.ApellidoPaterno,
                    Telefono = p.Persona.Telefono,
                    FechaNacimiento = p.Persona.FechaNacimiento,
                    Sexo = p.Persona.Sexo,
                    Foto = p.Persona.Foto,
                    EstadoCivil = p.Persona.EstadoCivil,
                    Ocupacion = p.Persona.Ocupacion,
                    NotasAdicionales = p.NotasAdicionales,
                    FechaRegistro = p.FechaRegistro
                })
                .FirstOrDefaultAsync();

            if (paciente == null)
            {
                return NotFound();
            }

            return Ok(paciente);
        }

        [HttpGet("pacientes")]
        public async Task<ActionResult<List<PacienteDto>>> GetPacientes()
        {
            var pacientes = await _context.Pacientes
                .Include(p => p.Persona) // Incluye la información de Persona
                .Select(p => new PacienteDto
                {
                    IdPaciente = p.Id,
                    Nombre = p.Persona.Nombre,
                    ApellidoMaterno = p.Persona.ApellidoMaterno,
                    ApellidoPaterno = p.Persona.ApellidoPaterno,
                    Telefono = p.Persona.Telefono,
                    FechaNacimiento = p.Persona.FechaNacimiento,
                    Sexo = p.Persona.Sexo,
                    Foto = p.Persona.Foto,
                    EstadoCivil = p.Persona.EstadoCivil,
                    Ocupacion = p.Persona.Ocupacion,
                    NotasAdicionales = p.NotasAdicionales,
                    FechaRegistro = p.FechaRegistro
                })
                .ToListAsync();

            return Ok(pacientes);
        }


        // api/account/register

        [AllowAnonymous]
        [HttpPost("register-paciente")]
        public async Task<ActionResult<string>> RegisterPaciente(RegisterPacienteDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Crear la entidad Persona
            var persona = new Persona
            {
                Nombre = registerDto.Nombre,
                ApellidoMaterno = registerDto.ApellidoMaterno,
                ApellidoPaterno = registerDto.ApellidoPaterno,
                Telefono = registerDto.Telefono,
                FechaNacimiento = registerDto.FechaNacimiento,
                Sexo = registerDto.Sexo,
                Foto = registerDto.Foto,
                EstadoCivil = registerDto.EstadoCivil,
                Ocupacion = registerDto.Ocupacion,
            };

            await _context.Personas.AddAsync(persona);
            await _context.SaveChangesAsync();

            // Crear la entidad Paciente y vincularla con Persona
            var paciente = new Paciente
            {
                IdPersona = persona.Id,
                FechaRegistro = DateTime.Now,
                // Otros campos específicos del paciente si existen
            };

            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();

            // Crear la relacion entre la entidad paciente y profesional
            var asignacionPaciente = new AsignacionPaciente
            {
                IdPaciente = paciente.Id,
                IdProfesional = registerDto.profesionalId,
                // Otros campos específicos del paciente si existen
            };

            await _context.AsignacionPacientes.AddAsync(asignacionPaciente);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Paciente registrado exitosamente!"
            });
        }

        // api/account/register

        [AllowAnonymous]
        [HttpPost("link-paciente")]
        public async Task<ActionResult<string>> LinkPaciente(AsignarPacienteDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Crear la relacion entre la entidad paciente y profesional
            var asignacionPaciente = new AsignacionPaciente
            {
                IdPaciente = registerDto.pacienteId,
                IdProfesional = registerDto.profesionalId,
                // Otros campos específicos del paciente si existen
            };

            await _context.AsignacionPacientes.AddAsync(asignacionPaciente);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Paciente asignado exitosamente!"
            });
        }

        [Authorize]
        [HttpDelete("eliminar-paciente/{id}")]
        public async Task<ActionResult> EliminarPaciente(int id)
        {
            // Busca el paciente por su ID
            var paciente = await _context.Pacientes
                .Include(p => p.Persona) // Incluir la entidad Persona relacionada
                .FirstOrDefaultAsync(p => p.Id == id);

            if (paciente == null)
            {
                return NotFound("Paciente no encontrado.");
            }

            // Elimina el paciente y la persona relacionada
            _context.Pacientes.Remove(paciente);
            _context.Personas.Remove(paciente.Persona);

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Paciente eliminado exitosamente.");
        }

        [Authorize]
        [HttpPut("modificar-paciente/{id}")]
        public async Task<ActionResult> ModificarPaciente(int id, [FromBody] UpdatePacienteDto updatePacienteDto)
        {
            // Busca el paciente por su ID
            var paciente = await _context.Pacientes
                .Include(p => p.Persona) // Incluir la entidad Persona relacionada
                .FirstOrDefaultAsync(p => p.Id == id);

            if (paciente == null)
            {
                return NotFound("Paciente no encontrado.");
            }

            // Actualiza la información de la entidad Persona
            paciente.Persona.Nombre = updatePacienteDto.Nombre;
            paciente.Persona.ApellidoPaterno = updatePacienteDto.ApellidoPaterno;
            paciente.Persona.ApellidoMaterno = updatePacienteDto.ApellidoMaterno;
            paciente.Persona.Telefono = updatePacienteDto.Telefono;
            paciente.Persona.FechaNacimiento = updatePacienteDto.FechaNacimiento;
            paciente.Persona.Sexo = updatePacienteDto.Sexo;
            paciente.Persona.Foto = updatePacienteDto.Foto;
            paciente.Persona.EstadoCivil = updatePacienteDto.EstadoCivil;
            paciente.Persona.Ocupacion = updatePacienteDto.Ocupacion;

            // Actualiza la lista de domicilios (si aplica)
            if (updatePacienteDto.Domicilios != null)
            {
                // Aquí puedes implementar la lógica para actualizar los domicilios asociados
                // Ejemplo: borrar los existentes y agregar los nuevos
            }

            // Marca las entidades como modificadas
            _context.Entry(paciente.Persona).State = EntityState.Modified;

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Paciente modificado exitosamente.");
        }


    }
}
