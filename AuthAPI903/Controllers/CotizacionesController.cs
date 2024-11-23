using AuthAPI903.Dtos;
using AuthAPI903.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using AuthAPI903.Data;

namespace AuthAPI903.Controladores
{
    [ApiController]
    [Route("api/[controller]")]
    public class CotizacionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CotizacionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CotizacionesDto>> Get()
        {
            var cotizacionDtos = _context.Cotizaciones
                .Select(c => new CotizacionesDto
                {
                    Id = c.Id,
                    ClienteId = c.ClienteId, // Incluye solo el ClienteId
                    Descripcion = c.Descripcion,
                    Precio = c.Precio,
                    Fecha = c.Fecha
                })
                .ToList();

            return Ok(cotizacionDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<CotizacionesDto> Get(int id)
        {
            var cotizacion = _context.Cotizaciones
                .Where(c => c.Id == id)
                .Select(c => new CotizacionesDto
                {
                    Id = c.Id,
                    ClienteId = c.ClienteId,
                    Descripcion = c.Descripcion,
                    Precio = c.Precio,
                    Fecha = c.Fecha
                })
                .FirstOrDefault();

            if (cotizacion == null)
            {
                return NotFound();
            }

            return Ok(cotizacion);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CotizacionesDto cotizacionDto)
        {
            // Verificar que el cliente exista
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == cotizacionDto.ClienteId);
            if (cliente == null)
            {
                return BadRequest("Cliente no encontrado.");
            }

            var cotizacion = new Cotizacion
            {
                ClienteId = cotizacionDto.ClienteId, // Relaciona el cliente por ID
                Descripcion = cotizacionDto.Descripcion,
                Precio = cotizacionDto.Precio,
                Fecha = cotizacionDto.Fecha
            };

            _context.Cotizaciones.Add(cotizacion);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = cotizacion.Id }, cotizacionDto);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CotizacionesDto cotizacionDto)
        {
            var cotizacion = _context.Cotizaciones.FirstOrDefault(c => c.Id == id);
            if (cotizacion == null)
            {
                return NotFound();
            }

            // Verificar que el cliente exista
            var cliente = _context.Clientes.FirstOrDefault(c => c.Id == cotizacionDto.ClienteId);
            if (cliente == null)
            {
                return BadRequest("Cliente no encontrado.");
            }


            cotizacion.Descripcion = cotizacionDto.Descripcion;
            cotizacion.Precio = cotizacionDto.Precio;
            cotizacion.Fecha = cotizacionDto.Fecha;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cotizacion = _context.Cotizaciones.FirstOrDefault(c => c.Id == id);
            if (cotizacion == null)
            {
                return NotFound();
            }

            _context.Cotizaciones.Remove(cotizacion);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
