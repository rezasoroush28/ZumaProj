﻿using Application.Common.Responses;
using MediatR;
using Zuma.Domain.Enums;

namespace Zuma.Application.Commands
{
    public sealed class DeleteToDoCommandRequest : IRequest<CommandResponse<DeleteToDoItemDto>>
    {
        public int ToDoItemId { get; set; }
    }

    public class DeleteToDoItemDto
    {
        
    }
}

