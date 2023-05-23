using System.Collections.Generic;


// This class stores info about the move
// which player made the move
// the position of the disc
// list of outflanked discs

public class MoveInfo
{
    public Player Player { get; set; }
    public Position Position { get; set; }
    public List<Position> Outflanked { get; set; }
}
