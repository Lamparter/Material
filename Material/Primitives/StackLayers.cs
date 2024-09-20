using System.Collections;

namespace Material.Primitives;
/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="max">Maximum of overlapping layers</param>
public class StackLayers<T>(int max = 4) : IEnumerable<T>
{
    internal readonly List<T> layers = [];
    internal readonly int max = max;

    /// <summary>
    /// Pops up the first layer.
    /// </summary>
    /// <returns>true if success; false if empty.</returns>
    public bool Pop()
    {
        try
        {
            layers.RemoveAt(0);
            return true;
        }
        catch (ArgumentOutOfRangeException) { return false; }
    }

    /// <summary>
    /// Pops up the layer in layers with given layer.
    /// </summary>
    /// <param name="layer"></param>
    /// <returns>true if layer is successfully removed; false if layer not found in layers.</returns>
    public bool Pop(T layer) => layers.Remove(layer);

    /// <summary>  
    /// Attempts to pop the first layer of type <typeparamref name="T"/> from a collection.  
    /// It invokes a delegate function <paramref name="onPop"/> with the layer as an argument.  
    /// If the delegate returns <c>true</c>, the pop operation is cancelled and the method returns <c>false</c>.  
    /// If the delegate returns <c>false</c>, the layer is popped from the collection and the method returns <c>true</c>.  
    /// If the collection is empty, the method returns <c>false</c> without invoking the delegate.  
    /// </summary>  
    /// <param name="onPop">A delegate function that takes a layer of type <typeparamref name="T"/> and returns a boolean.  
    /// It determines whether the pop operation should proceed based on the layer's properties or conditions.</param>  
    /// <returns><c>true</c> if the layer was successfully popped; <c>false</c> if the collection was empty or the pop operation was cancelled.</returns>  
    public bool Pop(Func<T, bool> onPop)
    {
        try
        {
            var layer = layers.ElementAt(0);
            var cancel = onPop(layer);

            if (cancel)
                return false;

            layers.RemoveAt(0);
            return true;
        }
        catch (ArgumentOutOfRangeException) { return false; }
    }

    /// <summary>
    /// Keep popping up the first layer until there is free space to push.
    /// </summary>
    internal void PopForFree()
    {
        while (layers.Count >= max)
            this.Pop();
    }

    /// <summary>
    /// Keep popping up the first layer until there is free space to push.
    /// </summary>
    /// <param name="onPop">A delegate function that takes a layer of type <typeparamref name="T"/> and returns a boolean.  
    /// It determines whether the pop operation should proceed based on the layer's properties or conditions.</param>  
    internal void PopForFree(Func<T, bool> onPop)
    {
        while (layers.Count >= max)
            this.Pop(onPop);
    }

    /// <summary>
    /// Pushes the layer to the end.
    /// </summary>
    /// <param name="layer">The specified layer to push.</param>
    public void Push(T layer)
    {
        this.PopForFree();

        layers.Add(layer);
    }

    /// <summary>
    /// Pushes the layer to the end.
    /// </summary>
    /// <param name="layer">The specified layer to push.</param>
    /// <param name="onPop">A delegate function that takes a layer of type <typeparamref name="T"/> and returns a boolean.  
    /// It determines whether the pop operation should proceed based on the layer's properties or conditions.</param>  
    public void Push(T layer, Func<T, bool> onPop)
    {
        this.PopForFree(onPop);

        layers.Add(layer);
    }

    /// <summary>
    /// Pops up all layers.
    /// </summary>
    public void Clear()
    {
        layers.Clear();
    }

    /// <summary>
    /// Pops up all layers.
    /// </summary>
    /// <param name="onPop">A delegate function that takes a layer of type <typeparamref name="T"/> and returns a boolean.  
    /// It determines whether the pop operation should proceed based on the layer's properties or conditions.</param>  
    public void Clear(Func<T, bool> onPop)
    {
        for (var i = 0; i < layers.Count; i++)
            if (!onPop(layers[i])) layers.Remove(layers[i]);
    }

    public IReadOnlyList<T> Layers => layers.AsReadOnly();
    public int Count => layers.Count;

    public T this[int index] => layers[index];

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)this.layers).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.layers).GetEnumerator();

}
