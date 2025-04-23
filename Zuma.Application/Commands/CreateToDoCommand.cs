using MediatR;
using Zuma.Domain.Enums;

namespace Zuma.Application.Commands
{
    public sealed class CreateToDoCommandRequest : IRequest<CreateToDoCommandResponse>
    {
        public string Title { get; set; } 
        public string Description { get; set; } 
        public ToDoStatus status { get; set; }
    }

    public sealed class CreateToDoCommandResponse 
    {

    }
}

