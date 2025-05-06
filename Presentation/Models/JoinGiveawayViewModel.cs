namespace Presentation.Models
{
    using DTO.Giveaway;
    using System.Collections.Generic;

    namespace Presentation.Models
    {
        public class JoinGiveawayViewModel
        {
            public List<GiveawayDto> Giveaways { get; set; } = new();
            public JoinModel JoinModel { get; set; } = new();
        }

        public class JoinModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public int GiveawayId { get; set; }
            public string? StatusMessage { get; set; }
        }
    }


}
