namespace Material.Primitives;
public class Ripple
{
    public float Size { get; set; }
    public float Percent { get; set; }
    public float Alpha { get; set; } = 1f;

    public readonly TaskCompletionSource RippleFinished = new();
}
