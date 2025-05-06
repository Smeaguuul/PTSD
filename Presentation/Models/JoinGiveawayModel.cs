using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class JoinGiveawayModel
    {
        public int GiveawayId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string? StatusMessage { get; set; }
    }

}
