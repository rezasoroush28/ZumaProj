using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuma.Domain.Interfaces.IRepositories;
using Zuma.Infrastructure.Context;

namespace Zuma.Infrastructure.Repositories
{
    public class ToDoItemRepository : IToDoItemRepository
    {

        private readonly ToDbContext _context;

        public ToDoItemRepository(ToDbContext context)
        {
            _context = context;
        }

        public async Task DeleteToDoItem(int id)
        {
            var itemCount =await _context.ToDoItems.Where(i => i.Id == id).ExecuteDeleteAsync();

            if (itemCount != 1)
                throw new KeyNotFoundException($"ToDoItem with Id {id} was not found.");
        }
    }
}
