using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zuma.Domain.Enums;

namespace Zuma.Domain.Interfaces.IRepositories
{
    public interface IToDoItemRepository
    {
        Task<List<ListAllToDoItemsDataDto>> ListAllToDoItems(ToDoStatus? status);
        Task DeleteToDoItem(int id);
        Task CreateToDoItem(string title, string description, int status);
        Task UpdateToDoItem(int id, string title, string description, int status);
    }

    public class ListAllToDoItemsDataDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ToDoStatus Status { get; set; }
    }
}
