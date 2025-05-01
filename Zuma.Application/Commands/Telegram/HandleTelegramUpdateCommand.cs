using Application.Common.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Zuma.Application.Commands.Telegram
{
    public class HandleTelegramUpdateCommandRequest : IRequest<CommandResponse<HandleTelegramUpdateDto>>
    {
        public Update TelUpdate { get; set; }
    }

    public class HandleTelegramUpdateDto
    {

    }
}
