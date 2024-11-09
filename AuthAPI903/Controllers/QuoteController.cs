using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class QuoteController : ControllerBase
{
    private static List<QuoteDto> quotes = new List<QuoteDto>();

    [HttpGet]
    public ActionResult<IEnumerable<QuoteDto>> Get()
    {
        return Ok(quotes);
    }

    [HttpPost]
    public ActionResult<QuoteDto> Post([FromBody] QuoteDto quote)
    {
        quotes.Add(quote);
        return CreatedAtAction(nameof(Get), new { id = quote.Id }, quote);
    }

    [HttpPut("{id}")]
    public ActionResult Put(string id, [FromBody] QuoteDto updatedQuote)
    {
        var quote = quotes.FirstOrDefault(q => q.Id == id);
        if (quote == null)
        {
            return NotFound();
        }

        quote.ClientName = updatedQuote.ClientName;
        quote.LicenseQuantity = updatedQuote.LicenseQuantity;
        quote.PlanType = updatedQuote.PlanType;
        quote.PlanPrice = updatedQuote.PlanPrice;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(string id)
    {
        var quote = quotes.FirstOrDefault(q => q.Id == id);
        if (quote == null)
        {
            return NotFound();
        }

        quotes.Remove(quote);
        return NoContent();
    }
}