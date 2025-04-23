using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuma.Application.Commands;

namespace Zuma.Application.CommandHandlers
{
    public class CreateToDoItemCommandHandler : IRequestHandler<CreateToDoCommandRequest, CreateToDoCommandResponse>
    {
        public async Task<CreateToDoCommandResponse> Handle(CreateToDoCommandRequest request, CancellationToken cancellationToken)
        {
            //Create the ToDoItem 
                
            return new CreateToDoCommandResponse
            {
                // Return the created ToDoItem
            };
        }
    }
}
