namespace JournalChallenge.Presentation.Controllers;

using JournalChallenge.Application.Journal.CreateNode;
using JournalChallenge.Application.Journal.DeleteNode;
using JournalChallenge.Application.Journal.RenameNode;
using JournalChallenge.Infrastructure.Core.Implementations;
using JournalChallenge.Presentation.DTOs;

using Microsoft.AspNetCore.Mvc;

[ApiController]
public class NodeController(
    ICreateNodeCommandHandler createHandler,
    IRenameNodeCommandHandler renameHandler,
    IDeleteNodeCommandHandler deleteHandler,
    IRestCustomResultsHandler resultsHandler) : ControllerBase
{
    [HttpPost("api.user.tree.node.create")]
    public async Task<IActionResult> CreateNode(
        [FromQuery] string treeName,
        [FromQuery] string nodeName,
        [FromQuery] long? parentNodeId,
        CancellationToken cancellationToken)
    {
        var command = new CreateNodeCommand(treeName, parentNodeId, nodeName);
        var result = await createHandler.HandleAsync(command, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(new EntityCreatedResponse(result.Value.ToString()));
        }

        throw resultsHandler.MatchProblem(result.Error);
    }

    [HttpPost("api.user.tree.node.rename")]
    public async Task<IActionResult> RenameNode(
        [FromQuery] long nodeId,
        [FromQuery] string newNodeName,
        CancellationToken cancellationToken)
    {
        var command = new RenameNodeCommand(nodeId, newNodeName);
        var result = await renameHandler.HandleAsync(command, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok();
        }

        throw resultsHandler.MatchProblem(result.Error);
    }

    [HttpPost("api.user.tree.node.delete")]
    public async Task<IActionResult> DeleteNode(
        [FromQuery] long nodeId,
        CancellationToken cancellationToken,
        [FromQuery] bool isForcedDeletion = false)
    {
        var command = new DeleteNodeCommand(nodeId, isForcedDeletion);
        var result = await deleteHandler.HandleAsync(command, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok();
        }

        throw resultsHandler.MatchProblem(result.Error);
    }
}
