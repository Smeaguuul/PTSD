using DTO.Giveaway;

public class AdminGiveawayPageViewModel
{
    public List<GiveawayDto> Giveaways { get; set; }
    public CreateGiveawayDto NewGiveaway { get; set; } = new();
}
