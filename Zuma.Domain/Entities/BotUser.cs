using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zuma.Domain.Entities
{
    public class BotUser
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
