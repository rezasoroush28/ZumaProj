using Application.Common.Responses;
using MediatR;
using Zuma.Domain.Enums;

namespace Zuma.Application.Commands
{
    public sealed class CreateToDoCommandRequest : IRequest<CommandResponse<CreateToDoItemDto>>
    {
        public string Title { get; set; } 
        public string Description { get; set; } 
        public ToDoStatus status { get; set; }
    }
    public class CreateToDoItemDto
    {

    }
    
}

