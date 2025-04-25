using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Zuma.Domain.Enums;

namespace ZumaProj.Api.Models
{
    public class CreateToDoItemModel
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; } = String.Empty;
        public ToDoStatus Status { get; set; } = ToDoStatus.JustMade;
    }
}
