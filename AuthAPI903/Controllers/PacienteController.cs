using AuthAPI903.Data;
using AuthAPI903.Dtos;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var pacientes = await _context.AsignacionPacientes
                .Where(p => p.Paciente.Estatus == true && p.IdProfesional == profesional.Id)
                .Include(p => p.Paciente.Persona) // Incluye la información de Persona
                .ToListAsync();

            var usuarios = await _context.Usuarios.ToListAsync();

            if (usuarios == null || !usuarios.Any())
            {
                return NotFound("No se encontraron pacientes asignados a este profesional.");
            }
            var personas = await _context.Personas.ToListAsync();

            if (personas == null || !personas.Any())
            {
                return NotFound("No se encontraron pacientes asignados a este profesional.");
            }



            // Extraer la lista de pacientes a partir de las asignaciones y devolver solo la información de Persona
            var pacientes2 = pacientes.Select(ap => new     
            {
                IdPaciente = ap.Paciente.Id,
                Nombre = ap.Paciente.Persona.Nombre,
                ApellidoPaterno = ap.Paciente.Persona.ApellidoPaterno,
                ApellidoMaterno = ap.Paciente.Persona.ApellidoMaterno,
                Telefono = ap.Paciente.Persona.Telefono,
                FechaNacimiento = ap.Paciente.Persona.FechaNacimiento,
                Sexo = ap.Paciente.Persona.Sexo,
                Foto = ap.Paciente.Persona.Foto
            }).ToList();




            return Ok(pacientes2);
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
                .FirstOrDefaultAsync(p => p.Id == id);

            if (paciente == null)
            {
                return NotFound("Paciente no encontrado.");
            }
            // Elimina el paciente y la persona relacionada
            paciente.Estatus = false;


            // Marca las entidades como modificadas
            _context.Entry(paciente).State = EntityState.Modified;

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Paciente eliminado exitosamente.");
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet("profesionales-asignados")]
        public async Task<ActionResult<List<ProfesionalPacienteDto>>> ObtenerProfesionalesAsignados()
        {
            // Obtener el ID del usuario autenticado
            var userId = _userManager.GetUserId(User);

            // Buscar el paciente asociado al usuario autenticado
            var paciente = await _context.Pacientes.Include(p => p.AsignacionPacientes)
                                             .ThenInclude(ap => ap.Profesional)
                                                 .ThenInclude(prof => prof.Usuario)
                                         .FirstOrDefaultAsync(p => p.Persona.Usuario.IdAppUser == userId);

            if (paciente == null)
            {
                return NotFound("Paciente no encontrado.");
            }

            // Obtener la lista de profesionales asignados al paciente
            var profesionales = paciente.AsignacionPacientes
                                        .Where(ap => ap.Profesional != null)
                                        .Select(ap => new ProfesionalPacienteDto
                                        {
                                            IdProfesional = ap.Profesional.Id,
                                            Nombre = ap.Profesional.Usuario.Persona.Nombre,
                                            ApellidoPaterno = ap.Profesional.Usuario.Persona.ApellidoPaterno,
                                            ApellidoMaterno = ap.Profesional.Usuario.Persona.ApellidoMaterno,
                                            Titulo = ap.Profesional.Titulo,
                                            Email = ap.Profesional.Usuario.Email
                                        })
                                        .ToList();

            if (!profesionales.Any())
            {
                return NotFound("No se encontraron profesionales asignados.");
            }

            return Ok(profesionales);
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
            paciente.Persona.EstadoCivil = updatePacienteDto.EstadoCivil;
            paciente.Persona.Ocupacion = updatePacienteDto.Ocupacion;
            paciente.NotasAdicionales = updatePacienteDto.NotasAdicionales;

            // Marca las entidades como modificadas
            _context.Entry(paciente.Persona).State = EntityState.Modified;

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Paciente modificado exitosamente." });
        }


    }
}
