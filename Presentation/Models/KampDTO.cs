namespace Presentation.Models;
public class KampDTO
{
    public int KampId { get; set; }
    public string Hold1 { get; set; }
    public string Hold2 { get; set; }

    public string PointHold1 { get; set; }
    public string PointHold2 { get; set; }

    public int GamesHold1 { get; set; }
    public int GamesHold2 { get; set; }

    public int SætHold1 { get; set; }
    public int SætHold2 { get; set; }

    public bool TieBreak { get; set; }
    public bool GoldenBallActive { get; set; }
}
