using Zuma.Domain.Enums;

namespace Zuma.Domain.Entities
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ToDoStatus status { get; set; }
    }
}
