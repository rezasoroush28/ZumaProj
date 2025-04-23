using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuma.Domain.Entities;

namespace Zuma.Infrastructure.Context
{
    public class ToDbContext : DbContext
    {
        public DbSet<ToDoItem> ToDoItems { get; set; }
    }

    // No OnModelCreating override needed for these simple constraints
}
