using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthAPI903.Data;
using AuthAPI903.Dtos;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestSharp;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager,
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

        // api/account/register

        [AllowAnonymous]
        [HttpPost("registerProfesional")]
        public async Task<ActionResult<string>> registerProfesional(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (registerDto.Roles is null)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                foreach (var role in registerDto.Roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }


            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Account Created Sucessfully!"
            });

        }

        // api/account/register
        [AllowAnonymous]
[HttpPost("register")]
public async Task<ActionResult<string>> register(RegisterDto2 registerDto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    var user = new AppUser
    {
        Email = registerDto.Email,
        FullName = registerDto.Email, // Cambiar por el nombre completo si es necesario
        UserName = registerDto.Email
    };

    var result = await _userManager.CreateAsync(user, registerDto.Password);

    if (!result.Succeeded)
    {
        return BadRequest(result.Errors);
    }

    if (registerDto.Roles is null)
    {
        await _userManager.AddToRoleAsync(user, "User");
    }
    else
    {
        foreach (var role in registerDto.Roles)
        {
            await _userManager.AddToRoleAsync(user, role);
        }
    }

    // AQUII
    // 1. Crear la entidad Persona
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
        Ocupacion = registerDto.Ocupacion
    };

    await _context.Personas.AddAsync(persona);
    await _context.SaveChangesAsync();

    // 2. Crear la entidad Usuario y vincularla con Persona y AppUser
    var usuario = new Usuario
    {
        IdAppUser = user.Id,
        Email = registerDto.Email,
        Contrasena = registerDto.Password, // Nota: Esto no debería almacenarse en texto plano
        TipoUsuario = "Profesional", // O el tipo que desees definir
        IdPersona = persona.Id,
        IdentificadorUnico = Guid.NewGuid().ToString()
    };

    await _context.Usuarios.AddAsync(usuario);
    await _context.SaveChangesAsync();

    // 3. Crear la entidad Profesional y vincularla con Usuario
    var profesional = new Profesional
    {
        Titulo = registerDto.Titulo,
        IdUsuario = usuario.Id
    };

    await _context.Profesionales.AddAsync(profesional);
    await _context.SaveChangesAsync();

    return Ok(new AuthResponseDto
    {
        IsSuccess = true,
        Message = "Account Created Successfully!"
    });
}
        [AllowAnonymous]
        [HttpPost("registerPaciente")]
        public async Task<ActionResult<string>> RegisterPaciente(RegisterPacienteDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. Crear la entidad AppUser
            var user = new AppUser
            {
                Email = registerDto.Email,
                FullName = $"{registerDto.Nombre} {registerDto.ApellidoPaterno} {registerDto.ApellidoMaterno}",
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Asignar el rol 'Paciente' al usuario recién creado
            await _userManager.AddToRoleAsync(user, "Paciente");

            // 2. Crear la entidad Persona
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
                Ocupacion = registerDto.Ocupacion
            };

            await _context.Personas.AddAsync(persona);
            await _context.SaveChangesAsync();

            // 3. Crear la entidad Usuario y vincularla con Persona y AppUser
            var usuario = new Usuario
            {
                IdAppUser = user.Id,
                Email = registerDto.Email,
                Contrasena = registerDto.Password, // Nota: Esto no debería almacenarse en texto plano
                TipoUsuario = "Paciente",
                IdPersona = persona.Id,
                IdentificadorUnico = Guid.NewGuid().ToString()
            };

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            // 4. Crear la entidad Paciente y vincularla con Usuario
            var paciente = new Paciente
            {
                IdPersona = persona.Id,
                FechaRegistro = DateTime.UtcNow,
                NotasAdicionales = registerDto.NotasAdicionales
            };

            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();




            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Paciente registrado exitosamente!"
            });
        }




        //api/account/login
        [AllowAnonymous]
        [HttpPost("login")]

        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return Unauthorized(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found with this email",
                });
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                return Unauthorized(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid Password."
                });
            }


            var token = GenerateToken(user);
            var refreshToken = GenerateRefreshToken();
            _ = int.TryParse(_configuration.GetSection("JWTSetting").GetSection("RefreshTokenValidityIn").Value!, out int RefreshTokenValidityIn);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(RefreshTokenValidityIn);
            await _userManager.UpdateAsync(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                IsSuccess = true,
                Message = "Login Success.",
                RefreshToken = refreshToken

            });


        }


        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdatePsychologist(int id, UpdatePsychologistDto updateDto)
        {


            var profesional = await _context.Profesionales
                .FirstOrDefaultAsync(p => p.Id == id);


            // Encuentra el usuario asociado al id
            var user = await _userManager.FindByIdAsync(profesional.Usuario.IdAppUser);

            

            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            if (profesional == null)
            {
                return NotFound("Profesional no encontrado.");
            }

            // Validar si el profesional está activo
            if (!profesional.Estatus)
            {
                return BadRequest("El profesional está inactivo.");
            }


            // Actualiza los datos de AppUser
            user.Email = updateDto.Email;
            user.FullName = updateDto.Nombre + " " + updateDto.ApellidoPaterno + " " + updateDto.ApellidoMaterno;
            user.UserName = updateDto.Email;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Encuentra la entidad Usuario relacionada y actualiza
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdAppUser == profesional.Usuario.IdAppUser);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario asociado no encontrado" });
            }
            usuario.Email = updateDto.Email;
            usuario.Contrasena = updateDto.Password;

            _context.Usuarios.Update(usuario);

            // Encuentra la entidad Persona relacionada y actualiza
            var persona = await _context.Personas.FindAsync(usuario.IdPersona);
            if (persona == null)
            {
                return NotFound(new { message = "Información de persona no encontrada" });
            }
            persona.Nombre = updateDto.Nombre;
            persona.ApellidoMaterno = updateDto.ApellidoMaterno;
            persona.ApellidoPaterno = updateDto.ApellidoPaterno;
            persona.Telefono = updateDto.Telefono;
            persona.FechaNacimiento = updateDto.FechaNacimiento;
            persona.Sexo = updateDto.Sexo;
            persona.Foto = updateDto.Foto;
            persona.EstadoCivil = updateDto.EstadoCivil;
            persona.Ocupacion = updateDto.Ocupacion;

            _context.Personas.Update(persona);

            // Encuentra la entidad Profesional relacionada y actualiza
             profesional = await _context.Profesionales.FirstOrDefaultAsync(p => p.IdUsuario == usuario.Id);
            if (profesional == null)
            {
                return NotFound(new { message = "Profesional no encontrado" });
            }
            profesional.Titulo = updateDto.Titulo;

            _context.Profesionales.Update(profesional);

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(new { message = "Datos del profesional actualizados correctamente" });
        }



        //api/account/login
        [AllowAnonymous]
        [HttpPost("login2")]

        public async Task<ActionResult<AuthResponseDto>> Login2(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return Unauthorized(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found with this email",
                });
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                return Unauthorized(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid Password."
                });
            }


            var token = GenerateToken(user);
            var refreshToken = GenerateRefreshToken();
            _ = int.TryParse(_configuration.GetSection("JWTSetting").GetSection("RefreshTokenValidityIn").Value!, out int RefreshTokenValidityIn);
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(RefreshTokenValidityIn);
            await _userManager.UpdateAsync(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                IsSuccess = true,
                Message = "Login Success.",
                RefreshToken = refreshToken

            });


        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]

        public async Task<ActionResult<AuthResponseDto>> RefreshToken(TokenDto tokenDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var principal = GetPrincipalFromExpiredToken(tokenDto.Token);
            var user = await _userManager.FindByEmailAsync(tokenDto.Email);

            if (principal is null || user is null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return BadRequest(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid client request"
                });

            var newJwtToken = GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            _ = int.TryParse(_configuration.GetSection("JWTSetting").GetSection("RefreshTokenValidityIn").Value!, out int RefreshTokenValidityIn);

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(RefreshTokenValidityIn);

            await _userManager.UpdateAsync(user);

            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Token = newJwtToken,
                RefreshToken = newRefreshToken,
                Message = "Refreshed token successfully"
            });




        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSetting").GetSection("securityKey").Value!)),
                ValidateLifetime = false


            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

            if (user is null)
            {
                return Ok(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User does not exist with this email"
                });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"http://localhost:4200/reset-password?email={user.Email}&token={WebUtility.UrlEncode(token)}";

            var client = new RestClient("https://send.api.mailtrap.io/api/send");

            var request = new RestRequest
            {
                Method = Method.Post,
                RequestFormat = DataFormat.Json
            };

            request.AddHeader("Authorization", "Bearer 62a57db5c125073400f28db0b418c6d0");
            request.AddJsonBody(new
            {
                from = new { email = "mailtrap@demomailtrap.com" },
                to = new[] { new { email = user.Email } },
                template_uuid = "b8bd784f-961a-4c6f-bdcd-fbddfc8ceabe",
                template_variables = new { user_email = user.Email, pass_reset_link = resetLink }
            });

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return Ok(new AuthResponseDto
                {
                    IsSuccess = true,
                    Message = "Email sent with password reset link. Please check your email."
                });
            }
            else
            {
                return BadRequest(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = response.Content!.ToString()
                });
            }

        }



        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            // resetPasswordDto.Token = WebUtility.UrlDecode(resetPasswordDto.Token);

            if (user is null)
            {
                return BadRequest(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User does not exist with this email"
                });
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new AuthResponseDto
                {
                    IsSuccess = true,
                    Message = "Password reset Successfully"
                });
            }

            return BadRequest(new AuthResponseDto
            {
                IsSuccess = false,
                Message = result.Errors.FirstOrDefault()!.Description
            });
        }


        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(changePasswordDto.Email);
            if (user is null)
            {
                return BadRequest(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User does not exist with this email"
                });
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new AuthResponseDto
                {
                    IsSuccess = true,
                    Message = "Password changed successfully"
                });
            }

            return BadRequest(new AuthResponseDto
            {
                IsSuccess = false,
                Message = result.Errors.FirstOrDefault()!.Description
            });
        }



        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        private string GenerateToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII
            .GetBytes(_configuration.GetSection("JWTSetting").GetSection("securityKey").Value!);

            var roles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims =
            [
                new (JwtRegisteredClaimNames.Email,user.Email??""),
                new (JwtRegisteredClaimNames.Name,user.FullName??""),
                new (JwtRegisteredClaimNames.NameId,user.Id ??""),
                new (JwtRegisteredClaimNames.Aud,
                _configuration.GetSection("JWTSetting").GetSection("validAudience").Value!),
                new (JwtRegisteredClaimNames.Iss,_configuration.GetSection("JWTSetting").GetSection("validIssuer").Value!)
            ];


            foreach (var role in roles)

            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);


        }

        //api/account/detail
        [HttpGet("detail")]
        public async Task<ActionResult<UserDetailDto>> GetUserDetail()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(currentUserId!);


            if (user is null)
            {
                return NotFound(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found"
                });
            }

            return Ok(new UserDetailDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = [.. await _userManager.GetRolesAsync(user)],
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                AccessFailedCount = user.AccessFailedCount,

            });

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailDto>>> GetUsers()
        {
            // Primero obtenemos la lista de usuarios con los datos básicos
            var users = await _userManager.Users.Select(u => new UserDetailDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                AccessFailedCount = u.AccessFailedCount,
                // Los roles se añadirán en el siguiente paso
            }).ToListAsync();

            // Luego, añadimos los roles para cada usuario
            foreach (var user in users)
            {
                var appUser = await _userManager.FindByIdAsync(user.Id);
                user.Roles = (await _userManager.GetRolesAsync(appUser)).ToList();
            }

            return Ok(users);
        }

        [HttpGet("profesionalDetail")]
        public async Task<ActionResult<UserDetailDto>> GetProfesionalDetail()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.Users
                .Include(u => u.Usuario)
                .ThenInclude(p => p.Profesional)
                .Include(u => u.Usuario.Persona)
                .FirstOrDefaultAsync(u => u.Id == currentUserId);

            if (user == null)
            {
                return NotFound(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found"
                });
            }

            var personaDto = user.Usuario.Persona == null ? null : new PersonaDto
            {
                Id = user.Usuario.Persona.Id,
                Nombre = user.Usuario.Persona.Nombre,
                Apellido = user.Usuario.Persona.ApellidoPaterno + " " + user.Usuario.Persona.ApellidoMaterno,
            };

            var profesionalDto = user.Usuario.Profesional == null ? null : new ProfesionalDto
            {
                Id = user.Usuario.Profesional.Id,
                Titulo = user.Usuario.Profesional.Titulo
            };

            return Ok(new UserDetailDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                AccessFailedCount = user.AccessFailedCount,
                Persona = personaDto,
                Profesional = profesionalDto
            });
        }


    }
}