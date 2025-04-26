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
    public class ToDoItemRepository : IToDoItemRepository
    {
        private readonly ToDoContext _context;

        public ToDoItemRepository(ToDoContext context)
        {
            _context = context;
        }

        public async Task CreateToDoItem(string title, string description, int status)
        {
            await _context.ToDoItems.AddAsync(new ToDoItem
            {
                Title = title,
                Description = description,
                Status = (ToDoStatus)status
            });

            await _context.SaveChangesAsync();
        }

        public async Task DeleteToDoItem(int id)
        {
            var itemCount = await _context.ToDoItems.Where(i => i.Id == id).ExecuteDeleteAsync();

            if (itemCount != 1)
                throw new KeyNotFoundException($"ToDoItem with Id {id} was not found.");
        }

        public async Task<List<ListAllToDoItemsDataDto>> ListAllToDoItems(ToDoStatus? status)
        {
            Expression<Func<ToDoItem, bool>> condition = i => true; // Default condition to include all items
            if (status != null)
            {
                condition = i => i.Status == status; // Filter by status if provided
            }

            var results = await _context.ToDoItems
                .Where(condition)
                .Select(i => new ListAllToDoItemsDataDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Status = (i.Status)
                })
                .ToListAsync();

            return results;
        }

        public async Task UpdateToDpItem(int id, string title, string description, int status)
        {
            var existingItem = await _context.ToDoItems.FindAsync(id);
            if (existingItem is null)
            {
                throw new Exception("ToDo item not found.");
            }

            existingItem.Title = title;
            existingItem.Description = description;
            existingItem.Status = (ToDoStatus)status;

            _context.ToDoItems.Update(existingItem);
            await _context.SaveChangesAsync();
        }
    }
}
