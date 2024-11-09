using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class PriceController : ControllerBase
{
    private static List<PriceDto> prices = new List<PriceDto>();

    [HttpGet]
    public ActionResult<IEnumerable<PriceDto>> Get()
    {
        return Ok(prices);
    }

    [HttpPost]
    public ActionResult<PriceDto> Post([FromBody] PriceDto price)
    {
        prices.Add(price);
        return CreatedAtAction(nameof(Get), new { id = price.Id }, price);
    }

    [HttpPut("{id}")]
    public ActionResult Put(string id, [FromBody] PriceDto updatedPrice)
    {
        var price = prices.FirstOrDefault(p => p.Id == id);
        if (price == null)
        {
            return NotFound();
        }

        price.PlanType = updatedPrice.PlanType;
        price.PlanPrice = updatedPrice.PlanPrice;
        price.Company = updatedPrice.Company;
        price.LicenseQuantity = updatedPrice.LicenseQuantity;
        price.ContractTime = updatedPrice.ContractTime;
        price.FinalPrice = updatedPrice.FinalPrice;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(string id)
    {
        var price = prices.FirstOrDefault(p => p.Id == id);
        if (price == null)
        {
            return NotFound();
        }

        prices.Remove(price);
        return NoContent();
    }
}