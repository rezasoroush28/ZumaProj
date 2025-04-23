using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuma.Domain.Entities;
using Zuma.Domain.Repositories;

namespace Zuma.Infrastructure.Repositories
{
    internal class ToDoItemRepository : IRepository<ToDoItem>
    {
        public Task<ToDoItem> CreateAsync(ToDoItem entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ToDoItem> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ToDoItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
