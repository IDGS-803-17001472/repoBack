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
    public class ListaPreciosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ListaPreciosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ListaPreciosDto>> Get()
        {
            var listaPreciosDtos = _context.ListaPrecios.Select(lp => new ListaPreciosDto
            {
                Id = lp.Id,
                TipoPlan = lp.TipoPlan,
                PrecioPlan = lp.PrecioPlan,
                Empresa = lp.Empresa,
                CantidadLicencias = lp.CantidadLicencias,
                DuracionContrato = lp.DuracionContrato,
                PrecioFinal = lp.PrecioFinal
            }).ToList();

            return Ok(listaPreciosDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<ListaPreciosDto> Get(int id)
        {
            var listaPrecios = _context.ListaPrecios.FirstOrDefault(lp => lp.Id == id);
            if (listaPrecios == null)
            {
                return NotFound();
            }

            var listaPreciosDto = new ListaPreciosDto
            {
                Id = listaPrecios.Id,
                TipoPlan = listaPrecios.TipoPlan,
                PrecioPlan = listaPrecios.PrecioPlan,
                Empresa = listaPrecios.Empresa,
                CantidadLicencias = listaPrecios.CantidadLicencias,
                DuracionContrato = listaPrecios.DuracionContrato,
                PrecioFinal = listaPrecios.PrecioFinal
            };

            return Ok(listaPreciosDto);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ListaPreciosDto listaPreciosDto)
        {
            var listaPrecios = new ListaPrecios
            {
                TipoPlan = listaPreciosDto.TipoPlan,
                PrecioPlan = listaPreciosDto.PrecioPlan,
                Empresa = listaPreciosDto.Empresa,
                CantidadLicencias = listaPreciosDto.CantidadLicencias,
                DuracionContrato = listaPreciosDto.DuracionContrato,
                PrecioFinal = listaPreciosDto.PrecioFinal
            };

            _context.ListaPrecios.Add(listaPrecios);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = listaPrecios.Id }, listaPreciosDto);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ListaPreciosDto listaPreciosDto)
        {
            var listaPrecios = _context.ListaPrecios.FirstOrDefault(lp => lp.Id == id);
            if (listaPrecios == null)
            {
                return NotFound();
            }

            listaPrecios.TipoPlan = listaPreciosDto.TipoPlan;
            listaPrecios.PrecioPlan = listaPreciosDto.PrecioPlan;
            listaPrecios.Empresa = listaPreciosDto.Empresa;
            listaPrecios.CantidadLicencias = listaPreciosDto.CantidadLicencias;
            listaPrecios.DuracionContrato = listaPreciosDto.DuracionContrato;
            listaPrecios.PrecioFinal = listaPreciosDto.PrecioFinal;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var listaPrecios = _context.ListaPrecios.FirstOrDefault(lp => lp.Id == id);
            if (listaPrecios == null)
            {
                return NotFound();
            }

            _context.ListaPrecios.Remove(listaPrecios);
            _context.SaveChanges();

            return NoContent();
        }
    }
}