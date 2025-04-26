using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Zuma.Domain.Enums;

namespace ZumaProj.Api.Models
{
    public class UpdateToDoItemModel
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } = String.Empty;
        public ToDoStatus Status { get; set; } = ToDoStatus.JustMade;
    }
}
