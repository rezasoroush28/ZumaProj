using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuma.Domain.Entities;
using Zuma.Domain.Enums;

namespace Zuma.Domain.Interfaces.IRepositories
{
    public interface IBotUserRepository
    {
        Task<BotUser> GetBotUserByChatId(long chatId);

        Task AddAsync(BotUser user);

    }
}
