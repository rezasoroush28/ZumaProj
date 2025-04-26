using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zuma.Application.Commands;
using Zuma.Domain.Entities;
using Zuma.Domain.Enums;
using ZumaProj.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class ToDosController : ControllerBase
{
    private readonly ISender _mediator;
    public ToDosController(ISender mediator) => _mediator = mediator;

    [HttpPost("CreateToDoItem")]
    public async Task<IActionResult> Create([FromForm] CreateToDoItemModel model)
    {
        var command = new CreateToDoCommandRequest
        {
            Title = model.Title,
            Description = model.Description,
            status = model.Status
        };

        var result = await _mediator.Send(command);
        if(result.Success is false)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPost("DeleteToDoItem")]

    public async Task<IActionResult> DeleteToDoItem(int toDoItemId)
    {
        var command = new DeleteToDoCommandRequest { ToDoItemId = toDoItemId };
        var result = await _mediator.Send(command);
        if(result.Success is false)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPost("GetAllToDoItems")]

    public async Task<IActionResult> GetAllToDoItems(ToDoStatus? status)
    {
        var command = new ListAllToDoItemsCommandRequest { Status = status };
        var result = await _mediator.Send(command);
        if (result.Success is false)
            return BadRequest(result.Message);

        return Ok(result);
    }



    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromForm] UpdateToDoItemModel updateToDoItemModel)
    {
        var command = new UpdateToDoItemCommandRequest
        {
            Id = updateToDoItemModel.Id,
            Title = updateToDoItemModel.Title,
            Description = updateToDoItemModel.Description,
            Status = updateToDoItemModel.Status
        };

        var result = await _mediator.Send(command);
        if (result.Success is false)
            return BadRequest(result.Message);

        return Ok(result);
    }
}
