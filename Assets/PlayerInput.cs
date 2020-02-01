public class PlayerInput
{
    public PlayerInput(bool Left, bool Right)
    {
        this.Left = Left;
        this.Right = Right;
    }
    public bool Left { get; private set; }
    public bool Right { get; private set; }
}
