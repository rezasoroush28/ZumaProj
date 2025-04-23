using MediatR;
using Zuma.Domain.Enums;

namespace Zuma.Application.Commands
{
    public sealed class DeleteToDoCommandRequest : IRequest<DeleteToDoCommandResponse>
    {
        public int ToDoIemId { get; set; }
    }

    public sealed class DeleteToDoCommandResponse
    {

    }
}

