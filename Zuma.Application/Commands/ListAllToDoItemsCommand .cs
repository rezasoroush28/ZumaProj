using Application.Common.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Zuma.Domain.Enums;

namespace Zuma.Application.Commands
{
    public sealed class ListAllToDoItemsCommand : IRequest<CommandResponse<List<ListAllToDoItemsDto>>>
    {
        public ToDoStatus? status { get; set; }
    }
    public class ListAllToDoItemsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ToDoStatus Status { get; set; }
    }
    
}

