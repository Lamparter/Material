namespace Material.Primitives;
public class EditTextChangedEventArgs(string text, TextRange range) : EventArgs
{
    public string Text { get; set; } = text;
    public TextRange SelectionRange { get; set; } = range;
}
