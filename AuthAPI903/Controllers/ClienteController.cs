using AuthAPI903.Data;
using AuthAPI903.Dtos;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthAPI903.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteDto>>> ObtenerClientes()
        {
            var clientes = await _context.Clientes
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    CorreoElectronico = c.CorreoElectronico,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    FechaDeRegistro = c.FechaDeRegistro
                })
                .ToListAsync();

            if (!clientes.Any())
                return NotFound("No se encontraron clientes.");

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> ObtenerClientePorId(int id)
        {
            var cliente = await _context.Clientes
                .Where(c => c.Id == id)
                .Select(c => new ClienteDto
                {
                    Id = c.Id,
                    CorreoElectronico = c.CorreoElectronico,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    FechaDeRegistro = c.FechaDeRegistro
                })
                .FirstOrDefaultAsync();

            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            return Ok(cliente);
        }

        [HttpPost("registrar-cliente")]
        public async Task<ActionResult> RegistrarCliente([FromBody] RegistrarClienteDto clienteDto)
        {
            var cliente = new Cliente
            {
                CorreoElectronico = clienteDto.CorreoElectronico,
                Nombre = clienteDto.Nombre,
                Apellido = clienteDto.Apellido,
                FechaDeRegistro = DateTime.Now
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cliente registrado exitosamente", clienteId = cliente.Id });
        }

        [HttpPut("{id}/actualizar")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] ActualizarClienteDto clienteDto)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            cliente.CorreoElectronico = clienteDto.CorreoElectronico ?? cliente.CorreoElectronico;
            cliente.Nombre = clienteDto.Nombre ?? cliente.Nombre;
            cliente.Apellido = clienteDto.Apellido ?? cliente.Apellido;

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict("Ocurrió un problema al actualizar el cliente.");
            }

            return Ok("Cliente actualizado exitosamente.");
        }

        [HttpDelete("{id}/eliminar")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return Ok("Cliente eliminado exitosamente.");
        }
    }
}
