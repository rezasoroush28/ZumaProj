using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuma.Domain.Entities;

namespace Zuma.Infrastructure.Context
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options)
               : base(options)
        {
        }
        public DbSet<ToDoItem> ToDoItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BotUser>(entity =>
            {
                entity.HasIndex(e => e.ChatId).IsUnique();
            });
        }
    }


    // No OnModelCreating override needed for these simple constraints
}
