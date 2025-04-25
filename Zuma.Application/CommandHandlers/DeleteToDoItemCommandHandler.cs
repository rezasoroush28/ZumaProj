using Application.Common.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuma.Application.Commands;
using Zuma.Domain.Interfaces.IRepositories;

namespace Zuma.Application.CommandHandlers
{
    public class DeleteToDoItemCommandHandler : IRequestHandler<DeleteToDoCommandRequest, CommandResponse<DeleteToDoItemDto>>
    {
        private readonly IToDoItemRepository _toDoItemRepository;

        public DeleteToDoItemCommandHandler(IToDoItemRepository toDoItemRepository)
        {
            _toDoItemRepository = toDoItemRepository;
        }

        public async Task<CommandResponse<DeleteToDoItemDto>> Handle(DeleteToDoCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _toDoItemRepository.DeleteToDoItem(request.ToDoIemId);
                return new CommandResponse<DeleteToDoItemDto>
                {
                    Success = true,
                    Result = new DeleteToDoItemDto(),
                    Message = "ToDo item deleted successfully."
                };
            }
            catch (Exception ex)
            {

                return CommandResponse<DeleteToDoItemDto>.Fail($"An error occurred while deleting the ToDo item: {ex.Message}");
            }

            
        }
    }
}
