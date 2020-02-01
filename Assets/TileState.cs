
public class TileState
{
    public Tile Player { get; }
    public Tile Right { get; }
    public Tile Left { get; }

    public TileState(Tile player, Tile right, Tile left)
    {
        this.Player = player;
        this.Right = right;
        this.Left = left;
    }
}