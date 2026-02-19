namespace JournalChallenge.Presentation.Controllers;

using JournalChallenge.Application.Journal.GetTree;
using JournalChallenge.Infrastructure.Core.Implementations;

using Microsoft.AspNetCore.Mvc;

[ApiController]
public class TreeController(
    IGetTreeQueryHandler getTreeHandler,
    IRestCustomResultsHandler resultsHandler) : ControllerBase
{
    [HttpPost("api.user.tree.get")]
    public async Task<IActionResult> GetTree(
        [FromQuery] string treeName,
        CancellationToken cancellationToken)
    {
        var query = new GetTreeQuery(treeName);
        var result = await getTreeHandler.HandleAsync(query, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        throw resultsHandler.MatchProblem(result.Error);
    }
}
