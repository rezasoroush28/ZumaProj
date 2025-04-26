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
    public class UpdateToDoItemCommandHandler : IRequestHandler<UpdateDoItemCommand, CommandResponse<UpdateDoItemDto>>
    {
        private readonly IToDoItemRepository _toDoItemRepository;

        public UpdateToDoItemCommandHandler(IToDoItemRepository toDoItemRepository)
        {
            _toDoItemRepository = toDoItemRepository;
        }

        public async Task<CommandResponse<UpdateDoItemDto>> Handle(UpdateDoItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _toDoItemRepository.UpdateToDoItem(request.Id, request.Description, request.Description, (int)request.Status);
                return new CommandResponse<UpdateDoItemDto>
                {
                    Success = true,
                    Result = new UpdateDoItemDto(),
                    Message = "ToDo item Updated successfully."
                };
            }
            catch (Exception ex)
            {
                return CommandResponse<UpdateDoItemDto>.Fail($"An error occurred while creating the ToDo item: {ex.Message}");
            }
            throw new NotImplementedException();
        }
    }
}
