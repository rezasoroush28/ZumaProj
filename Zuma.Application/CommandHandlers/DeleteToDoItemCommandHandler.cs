using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuma.Application.Commands;

namespace Zuma.Application.CommandHandlers
{
    public class DeleteToDoItemCommandHandler : IRequestHandler<DeleteToDoCommandRequest, DeleteToDoCommandResponse>
    {
        public async Task<DeleteToDoCommandResponse> Handle(DeleteToDoCommandRequest request, CancellationToken cancellationToken)
        {
            //Create the ToDoItem 
                
            return new DeleteToDoCommandResponse
            {
                // Return the created ToDoItem
            };
        }
    }
}
