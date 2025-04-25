using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zuma.Domain.Interfaces.IRepositories
{
    public interface IToDoItemRepository 
    {
        Task DeleteToDoItem(int id);
        Task CreateToDoItem(string title, string description, int status);
    }
}
