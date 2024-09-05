namespace Material.Primitives;
public class SelectionRangeChangedEventArgs(TextRange range) : EventArgs
{
    public TextRange SelectionRange { get; set; } = range;
}
