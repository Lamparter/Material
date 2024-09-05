namespace Material.Interfaces;

public interface IIconElement : IElement, IDisposable
{
    string IconData { get; set; }
    PathF IconPath { get; set; }
    Color IconColor { get; set; }

    public static readonly BindableProperty IconDataProperty = BindableProperty.Create(
        nameof(IconData),
        typeof(string),
        typeof(IIconElement),
        default,
        propertyChanged: (bo, ov, nv) =>
        {
            ((IIconElement)bo).IconPath = PathBuilder.Build((string)nv);
            ((IElement)bo).InvalidateMeasure();
        }
    );

    public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
        nameof(IconColor),
        typeof(Color),
        typeof(IIconElement),
        default,
        propertyChanged: (bo, ov, nv) => ((IElement)bo).OnPropertyChanged()
    );
}
