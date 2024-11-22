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
    public class EmpresaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpresaController(AppDbContext context)
        {
            _context = context;
        }

        // API para consultar todos los clientes y sus datos
        [HttpGet("clientes")]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetClientes()
        {
            return await _context.Empresas.ToListAsync();
        }

        // API para registrar una nueva empresa
        [HttpPost("registrar")]
        public async Task<ActionResult<Empresa>> RegisterEmpresa(Empresa empresa)
        {
            // Añadir la empresa a la base de datos
            _context.Empresas.Add(empresa);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Retornar la respuesta con el nuevo id autogenerado
            return CreatedAtAction(nameof(GetClientes), new { id = empresa.Id }, empresa);
        }

        // API para traer todas las empresas
        [HttpGet("todas")]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetTodasEmpresas()
        {
            return await _context.Empresas.ToListAsync();
        }

        // API para actualizar el estatus mediante el idEmpresa y el estatus
        [HttpPut("actualizar-estatus/{id}")]
        public async Task<IActionResult> UpdateEstatus(int id, [FromBody] int estatus)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null) return NotFound();

            empresa.Estatus = estatus;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }


}
