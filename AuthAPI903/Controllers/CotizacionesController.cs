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
            var cotizacionDtos = _context.Cotizaciones.Select(c => new CotizacionesDto
            {
                Id = c.Id,
                Cliente = c.Cliente,
                Descripcion = c.Descripcion,
                Precio = c.Precio,
                Fecha = c.Fecha
            }).ToList();

            return Ok(cotizacionDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<CotizacionesDto> Get(int id)
        {
            var cotizacion = _context.Cotizaciones.FirstOrDefault(c => c.Id == id);
            if (cotizacion == null)
            {
                return NotFound();
            }

            var cotizacionDto = new CotizacionesDto
            {
                Id = cotizacion.Id,
                Cliente = cotizacion.Cliente,
                Descripcion = cotizacion.Descripcion,
                Precio = cotizacion.Precio,
                Fecha = cotizacion.Fecha
            };

            return Ok(cotizacionDto);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CotizacionesDto cotizacionDto)
        {
            var cotizacion = new Cotizacion
            {
                Cliente = cotizacionDto.Cliente,
                Descripcion = cotizacionDto.Descripcion,
                Precio = cotizacionDto.Precio,
                Fecha = cotizacionDto.Fecha
            };

            _context.Cotizaciones.Add(cotizacion);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = cotizacion.Id }, cotizacionDto);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CotizacionesDto cotizacionesDto)
        {
            var cotizacion = _context.Cotizaciones.FirstOrDefault(c => c.Id == id);
            if (cotizacion == null)
            {
                return NotFound();
            }

            cotizacion.Cliente = cotizacionesDto.Cliente;
            cotizacion.Descripcion = cotizacionesDto.Descripcion;
            cotizacion.Precio = cotizacionesDto.Precio;
            cotizacion.Fecha = cotizacionesDto.Fecha;

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