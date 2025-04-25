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
    public class ListAllToDoItemsCommandHandler : IRequestHandler<ListAllToDoItemsCommand, CommandResponse<List<ListAllToDoItemsDto>>>
    {
        private readonly IToDoItemRepository _toDoItemRepository;

        public ListAllToDoItemsCommandHandler(IToDoItemRepository toDoItemRepository)
        {
            _toDoItemRepository = toDoItemRepository;
        }

        public async Task<CommandResponse<List<ListAllToDoItemsDto>>> Handle(ListAllToDoItemsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dataResults = await _toDoItemRepository.ListAllToDoItems(request.status);
                var results = dataResults.Select(d => new ListAllToDoItemsDto
                {
                    Id = d.Id,
                    Title = d.Title,
                    Description = d.Description,
                    Status = d.Status,  
                }).ToList();

                return new CommandResponse<List<ListAllToDoItemsDto>>
                {
                    Success = true,
                    Result = results,
                    Message = "ToDo items retrieved successfully."
                };


            }
            catch (Exception ex)
            {
                return CommandResponse<List<ListAllToDoItemsDto>>.Fail($"An error occurred while retrieving the ToDo items: {ex.Message}");
            }
        }
    }
}
