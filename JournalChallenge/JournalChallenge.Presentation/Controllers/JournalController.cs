namespace JournalChallenge.Presentation.Controllers;

using JournalChallenge.Application.Journal.GetRange;
using JournalChallenge.Application.Journal.GetSingle;

using Microsoft.AspNetCore.Mvc;

[ApiController]
public class JournalController(
    IGetJournalRangeQueryHandler getRangeHandler,
    IGetJournalSingleQueryHandler getSingleHandler) : ControllerBase
{
    [HttpPost("api.user.journal.getRange")]
    public async Task<IActionResult> GetRange(
        [FromQuery] int skip,
        [FromQuery] int take,
        [FromBody] JournalFilter? filter,
        CancellationToken cancellationToken)
    {
        var query = new GetJournalRangeQuery(skip, take, filter);
        var result = await getRangeHandler.HandleAsync(query, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error.Error.Message);
    }

    [HttpPost("api.user.journal.getSingle")]
    public async Task<IActionResult> GetSingle(
        [FromQuery] long id,
        CancellationToken cancellationToken)
    {
        var query = new GetJournalSingleQuery(id);
        var result = await getSingleHandler.HandleAsync(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error.Error.Message);
    }
}
