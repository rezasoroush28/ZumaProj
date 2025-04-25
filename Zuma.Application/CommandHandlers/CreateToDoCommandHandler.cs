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
    public class CreateToDoCommandHandler : IRequestHandler<CreateToDoCommandRequest, CommandResponse<CreateToDoItemDto>>
    {
        private readonly IToDoItemRepository _toDoItemRepository;

        public CreateToDoCommandHandler(IToDoItemRepository toDoItemRepository)
        {
            _toDoItemRepository = toDoItemRepository;
        }

        public async Task<CommandResponse<CreateToDoItemDto>> Handle(CreateToDoCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _toDoItemRepository.CreateToDoItem(request.Title, request.Description, (int)request.status);
                return new CommandResponse<CreateToDoItemDto>
                {
                    Success = true,
                    Result = new CreateToDoItemDto(),
                    Message = "ToDo item created successfully."
                };
            }
            catch (Exception ex)
            {
                return CommandResponse<CreateToDoItemDto>.Fail($"An error occurred while creating the ToDo item: {ex.Message}");
            }



        }
    }
}
