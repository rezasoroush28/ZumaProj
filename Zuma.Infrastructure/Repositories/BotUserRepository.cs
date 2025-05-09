using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Zuma.Domain.Entities;
using Zuma.Domain.Enums;
using Zuma.Domain.Interfaces.IRepositories;
using Zuma.Infrastructure.Context;

namespace Zuma.Infrastructure.Repositories
{
    public class BotUserRepository : IBotUserRepository
    {
        private readonly ToDoContext _context;

        public BotUserRepository(ToDoContext context)
        {
            _context = context;
        }

        public async Task<BotUser> GetBotUserByChatId(long chatId)
        {
            return await _context.BotUsers.AsNoTracking().Where(b => b.ChatId == chatId).FirstOrDefaultAsync();
        }

        public async Task AddAsync(BotUser user)
        {
            try
            {
                await _context.BotUsers.AddAsync(user);
                _context.SaveChanges();
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }

        }




    }
}
