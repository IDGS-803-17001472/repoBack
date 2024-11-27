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
    public class OportunidadVentaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OportunidadVentaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OportunidadVentaDto>> Get()
        {
            var oportunidadVentaDtos = _context.OportunidadesVenta.Select(o => new OportunidadVentaDto
            {
                Id = o.Id,
                NombreCliente = o.NombreCliente,
                Descripcion = o.Descripcion,
                ValorEstimado = o.ValorEstimado,
                FechaCierre = o.FechaCierre,
                Estado = o.Estado
            }).ToList();

            return Ok(oportunidadVentaDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<OportunidadVentaDto> Get(int id)
        {
            var oportunidadVenta = _context.OportunidadesVenta.FirstOrDefault(o => o.Id == id);
            if (oportunidadVenta == null)
            {
                return NotFound();
            }

            var oportunidadVentaDto = new OportunidadVentaDto
            {
                Id = oportunidadVenta.Id,
                NombreCliente = oportunidadVenta.NombreCliente,
                Descripcion = oportunidadVenta.Descripcion,
                ValorEstimado = oportunidadVenta.ValorEstimado,
                FechaCierre = oportunidadVenta.FechaCierre,
                Estado = oportunidadVenta.Estado
            };

            return Ok(oportunidadVentaDto);
        }

        [HttpPost]
        public IActionResult Post([FromBody] OportunidadVentaDto oportunidadVentaDto)
        {
            var oportunidadVenta = new OportunidadVenta
            {
                NombreCliente = oportunidadVentaDto.NombreCliente,
                Descripcion = oportunidadVentaDto.Descripcion,
                ValorEstimado = oportunidadVentaDto.ValorEstimado,
                FechaCierre = oportunidadVentaDto.FechaCierre,
                Estado = oportunidadVentaDto.Estado
            };

            _context.OportunidadesVenta.Add(oportunidadVenta);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = oportunidadVenta.Id }, oportunidadVentaDto);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] OportunidadVentaDto oportunidadVentaDto)
        {
            var oportunidadVenta = _context.OportunidadesVenta.FirstOrDefault(o => o.Id == id);
            if (oportunidadVenta == null)
            {
                return NotFound();
            }

            oportunidadVenta.NombreCliente = oportunidadVentaDto.NombreCliente;
            oportunidadVenta.Descripcion = oportunidadVentaDto.Descripcion;
            oportunidadVenta.ValorEstimado = oportunidadVentaDto.ValorEstimado;
            oportunidadVenta.FechaCierre = oportunidadVentaDto.FechaCierre;
            oportunidadVenta.Estado = oportunidadVentaDto.Estado;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var oportunidadVenta = _context.OportunidadesVenta.FirstOrDefault(o => o.Id == id);
            if (oportunidadVenta == null)
            {
                return NotFound();
            }

            _context.OportunidadesVenta.Remove(oportunidadVenta);
            _context.SaveChanges();

            return NoContent();
        }
    }
}