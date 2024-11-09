using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class OpportunityController : ControllerBase
{
    private static List<OpportunityDto> opportunities = new List<OpportunityDto>();

    [HttpGet]
    public ActionResult<IEnumerable<OpportunityDto>> Get()
    {
        return Ok(opportunities);
    }

    [HttpPost]
    public ActionResult<OpportunityDto> Post([FromBody] OpportunityDto opportunity)
    {
        opportunities.Add(opportunity);
        return CreatedAtAction(nameof(Get), new { id = opportunity.Id }, opportunity);
    }

    [HttpPut("{id}")]
    public ActionResult Put(string id, [FromBody] OpportunityDto updatedOpportunity)
    {
        var opportunity = opportunities.FirstOrDefault(o => o.Id == id);
        if (opportunity == null)
        {
            return NotFound();
        }

        opportunity.ClientName = updatedOpportunity.ClientName;
        opportunity.LicenseQuantity = updatedOpportunity.LicenseQuantity;
        opportunity.Status = updatedOpportunity.Status;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(string id)
    {
        var opportunity = opportunities.FirstOrDefault(o => o.Id == id);
        if (opportunity == null)
        {
            return NotFound();
        }

        opportunities.Remove(opportunity);
        return NoContent();
    }
}