public class PlayerInput
{
    public PlayerInput(bool Left, bool Right, bool Interact)
    {
        this.Left = Left;
        this.Right = Right;
        this.Interact = Interact;
    }
    public bool Left { get; private set; }
    public bool Right { get; private set; }
    public bool Interact { get; private set; }
}
