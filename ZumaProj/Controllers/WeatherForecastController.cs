using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zuma.Application.Commands;
using Zuma.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
public class ToDosController : ControllerBase
{
    private readonly ISender _mediator;
    public ToDosController(ISender mediator) => _mediator = mediator;

    //[HttpPost]
    //public async Task<IActionResult> Create(CreateToDoCommand cmd)
    //    => CreatedAtAction(null, new { id = await _mediator.Send(cmd) }, null);

    [HttpPost("DeleteToDoItem")]

    public async Task<IActionResult> DeleteToDoItem( int toDoItemId)
    {
        var command = new DeleteToDoCommandRequest { ToDoIemId = toDoItemId};
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    //    [HttpPut("{id:guid}")]
    //    public async Task<IActionResult> Update(Guid id, UpdateToDoCommand cmd)
    //    {
    //        if (id != cmd.Id) return BadRequest();
    //        await _mediator.Send(cmd);
    //        return NoContent();
    //    }

    //    [HttpDelete("{id:guid}")]
    //    public async Task<IActionResult> Delete(Guid id)
    //    {
    //        await _mediator.Send(new DeleteToDoCommand(id));
    //        return NoContent();
    //    }
}
