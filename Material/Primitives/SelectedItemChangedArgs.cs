namespace Material.Primitives;

public class SelectedItemChangedArgs<T>(T selectedItem) : EventArgs
{
    public T SelectedItem { get; set; } = selectedItem;
}
