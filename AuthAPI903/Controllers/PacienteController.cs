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
    [Authorize(Roles = "profesional")]
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

        [Authorize]
        [HttpGet("profesional/pacientes")]
        public async Task<ActionResult<List<PersonaPacienteDto>>> ObtenerPacientesPorProfesional()
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

            int id = profesional.Id;
            // Busca las asignaciones de pacientes para el profesional dado
            var pacientes = await _context.Pacientes
                .Where(p=> p.Estatus == true)
                .Include(p => p.Persona) // Incluye la información de Persona
                .ToListAsync();

            if (pacientes == null || !pacientes.Any())
            {
                return NotFound("No se encontraron pacientes asignados a este profesional.");
            }

            // Extraer la lista de pacientes a partir de las asignaciones y devolver solo la información de Persona
            var pacientes2 = pacientes.Select(ap => new PersonaPacienteDto
            {
                IdPaciente = ap.Id,
                Nombre = ap.Persona.Nombre,
                ApellidoPaterno = ap.Persona.ApellidoPaterno,
                ApellidoMaterno = ap.Persona.ApellidoMaterno,
                Telefono = ap.Persona.Telefono,
                FechaNacimiento = ap.Persona.FechaNacimiento,
                Sexo = ap.Persona.Sexo,
            }).ToList();

            return Ok(pacientes2);
        }



        [Authorize]
        [HttpGet("paciente/{id}/profesionales")]
        public async Task<ActionResult<List<Profesional>>> ObtenerProfesionalesPorPaciente(int id)
        {
            // Busca las asignaciones de profesionales para el paciente dado
            var asignaciones = await _context.AsignacionPacientes
                .Where(ap => ap.IdPaciente == id && ap.Paciente.Estatus == true)
                .Include(ap => ap.Profesional) // Incluye la información de los profesionales
                .ThenInclude(p => p.Usuario) // Incluye la información de Usuario si es necesario
                .ThenInclude(u => u.Persona) // Incluye la información de Persona si es necesario
                .ToListAsync();

            if (asignaciones == null || !asignaciones.Any())
            {
                return NotFound("No se encontraron profesionales asignados a este paciente.");
            }

            // Extraer la lista de profesionales a partir de las asignaciones
            var profesionales = asignaciones.Select(ap => ap.Profesional).Where(p => p.Estatus == true).ToList();

            return Ok(profesionales);
        }


        //// api/account/register

        //[AllowAnonymous]
        //[HttpPost("register-pacienteNoUsar")]
        //public async Task<ActionResult<string>> RegisterPaciente(RegisterPacienteDto registerDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Crear la entidad Persona
        //    var persona = new Persona
        //    {
        //        Nombre = registerDto.Nombre,
        //        ApellidoMaterno = registerDto.ApellidoMaterno,
        //        ApellidoPaterno = registerDto.ApellidoPaterno,
        //        Telefono = registerDto.Telefono,
        //        FechaNacimiento = registerDto.FechaNacimiento,
        //        Sexo = registerDto.Sexo,
        //        Foto = registerDto.Foto,
        //        EstadoCivil = registerDto.EstadoCivil,
        //        Ocupacion = registerDto.Ocupacion,
        //    };

        //    await _context.Personas.AddAsync(persona);
        //    await _context.SaveChangesAsync();

        //    // Crear la entidad Paciente y vincularla con Persona
        //    var paciente = new Paciente
        //    {
        //        IdPersona = persona.Id,
        //        FechaRegistro = DateTime.Now,
        //        // Otros campos específicos del paciente si existen
        //    };

        //    await _context.Pacientes.AddAsync(paciente);
        //    await _context.SaveChangesAsync();

        //    // Crear la relacion entre la entidad paciente y profesional
        //    var asignacionPaciente = new AsignacionPaciente
        //    {
        //        IdPaciente = paciente.Id,
        //        IdProfesional = registerDto.profesionalId,
        //        // Otros campos específicos del paciente si existen
        //    };

        //    await _context.AsignacionPacientes.AddAsync(asignacionPaciente);
        //    await _context.SaveChangesAsync();

        //    return Ok(new AuthResponseDto
        //    {
        //        IsSuccess = true,
        //        Message = "Paciente registrado exitosamente!"
        //    });
        //}


        [HttpPost("link")]
        public async Task<ActionResult<string>> LinkPaciente(AsignarPacienteDto registerDto)
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

            // Verificar si el paciente existe y su estatus
            var paciente = await _context.Pacientes
                                         .FirstOrDefaultAsync(p => p.Id == registerDto.pacienteId);

            if (paciente == null)
            {
                return NotFound("El paciente no fue encontrado.");
            }

            if (!paciente.Estatus)
            {
                return BadRequest("El estatus del paciente debe ser activo para asignarlo.");
            }

            // Verificar si la asignación ya existe
            var asignacionExiste = await _context.AsignacionPacientes
                                                  .AnyAsync(ap => ap.IdPaciente == registerDto.pacienteId && ap.IdProfesional == profesional.Id);

            if (asignacionExiste)
            {
                return Conflict("El paciente ya está asignado a este profesional.");
            }

            // Crear la relación entre la entidad paciente y profesional
            var asignacionPaciente = new AsignacionPaciente
            {
                IdPaciente = registerDto.pacienteId,
                IdProfesional = profesional.Id
            };

            await _context.AsignacionPacientes.AddAsync(asignacionPaciente);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Paciente asignado exitosamente!"
            });
        }



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
            paciente.Estatus = false;

            _context.Entry(paciente).State = EntityState.Modified;


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
