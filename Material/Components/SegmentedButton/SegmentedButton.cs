﻿using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Material;

[ContentProperty(nameof(Items))]
public class SegmentedButton
    : TouchGraphicsView,
        IItemsElement<SegmentedItem>,
        IItemsSourceElement,
        IFontElement,
        IOutlineElement,
        IElement,
        IBackgroundElement,
        IShapeElement,
        IStateLayerElement,
        IRippleElement,
        IICommandElement,
        IVisualTreeElement,
        IDisposable
{
    protected override void ChangeVisualState()
    {
        if (this.ViewState is ViewState.Hovered or ViewState.Pressed)
        {
            var index = (int)(this.LastTouchPoint.X / (this.Bounds.Width / this.Items.Count));
            for (var i = 0; i < this.Items.Count; i++)
            {
                var item = this.Items[i];
                item.ViewState = i == index ? this.ViewState : ViewState.Normal;
            }
        }
        else
        {
            foreach (var item in this.Items)
                item.ViewState = this.ViewState;
        }
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if (this.Items != null)
        {
            foreach (var item in this.Items)
            {
                SetInheritedBindingContext(item, this.BindingContext);
            }
        }
    }

    public static readonly BindableProperty ItemsProperty =
        IItemsElement<SegmentedItem>.ItemsProperty;

    public static readonly BindableProperty ItemsSourceProperty =
        IItemsSourceElement.ItemsSourceProperty;

    public static readonly BindableProperty MultiSelectModeProperty = BindableProperty.Create(
        nameof(MultiSelectMode),
        typeof(bool),
        typeof(SegmentedButton),
        default
    );

    public static readonly BindableProperty FontColorProperty = IFontElement.FontColorProperty;
    public static readonly BindableProperty FontSizeProperty = IFontElement.FontSizeProperty;
    public static readonly BindableProperty FontFamilyProperty = IFontElement.FontFamilyProperty;
    public static readonly BindableProperty FontWeightProperty = IFontElement.FontWeightProperty;
    public static readonly BindableProperty FontIsItalicProperty =
        IFontElement.FontIsItalicProperty;

    public static readonly BindableProperty OutlineWidthProperty =
        IOutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineColorProperty =
        IOutlineElement.OutlineColorProperty;

    public ObservableCollection<SegmentedItem> Items
    {
        get => (ObservableCollection<SegmentedItem>)this.GetValue(ItemsProperty);
        set => this.SetValue(ItemsProperty, value);
    }

    public IList ItemsSource
    {
        get => (IList)this.GetValue(ItemsSourceProperty);
        set => this.SetValue(ItemsSourceProperty, value);
    }

    void IItemsElement<SegmentedItem>.OnItemsCollectionChanged(
        object sender,
        NotifyCollectionChangedEventArgs e
    )
    {
        if (e.OldItems != null)
        {
            var index = e.OldStartingIndex;
            foreach (SegmentedItem item in e.OldItems)
            {
                this.OnChildRemoved(item, index);
                VisualDiagnostics.OnChildRemoved(this, item, index);
                SetInheritedBindingContext(item, null);
                index++;
            }
        }

        if (e.NewItems != null)
        {
            var index = e.NewStartingIndex;
            foreach (SegmentedItem item in e.NewItems)
            {
                this.AddLogicalChild(item);
                if (this.BindingContext != null)
                {
                    SetInheritedBindingContext(item, this.BindingContext);
                }
            }
        }
        ((IElement)this).InvalidateMeasure();
    }

    void IItemsSourceElement.OnItemsSourceCollectionChanged(
        object sender,
        NotifyCollectionChangedEventArgs e
    )
    {
        if (e.OldItems != null)
        {
            var index = e.OldStartingIndex;
            foreach (string item in e.OldItems)
            {
                this.Items.RemoveAt(index);
                index++;
            }
        }

        if (e.NewItems != null)
        {
            var index = e.NewStartingIndex;
            foreach (string item in e.NewItems)
            {
                this.Items.Insert(index, new SegmentedItem { Text = item });
                index++;
            }
        }
    }

    public bool MultiSelectMode
    {
        get => (bool)this.GetValue(MultiSelectModeProperty);
        set => this.SetValue(MultiSelectModeProperty, value);
    }

    public Color FontColor
    {
        get => (Color)this.GetValue(FontColorProperty);
        set => this.SetValue(FontColorProperty, value);
    }
    public float FontSize
    {
        get => (float)this.GetValue(FontSizeProperty);
        set => this.SetValue(FontSizeProperty, value);
    }
    public string FontFamily
    {
        get => (string)this.GetValue(FontFamilyProperty);
        set => this.SetValue(FontFamilyProperty, value);
    }
    public FontWeight FontWeight
    {
        get => (FontWeight)this.GetValue(FontWeightProperty);
        set => this.SetValue(FontWeightProperty, value);
    }
    public bool FontIsItalic
    {
        get => (bool)this.GetValue(FontIsItalicProperty);
        set => this.SetValue(FontIsItalicProperty, value);
    }

    public Color OutlineColor
    {
        get => (Color)this.GetValue(OutlineColorProperty);
        set => this.SetValue(OutlineColorProperty, value);
    }

    public int OutlineWidth
    {
        get => (int)this.GetValue(OutlineWidthProperty);
        set => this.SetValue(OutlineWidthProperty, value);
    }

    public IEnumerable<SegmentedItem> SelectedItems
    {
        get
        {
            foreach (var item in this.Items)
            {
                if (item.IsSelected)
                {
                    yield return item;
                }
            }
        }
    }

    public event EventHandler<SelectedItemsChangedArgs<SegmentedItem>> SelectedItemsChanged;

    public SegmentedButton()
    {
        this.Clicked += this.OnClicked;
        this.MoveHoverInteraction += this.OnMoveHoverInteraction;
        this.Drawable = new SegmentedButtonDrawable(this);
    }

    private void OnClicked(object sender, TouchEventArgs e)
    {
        var index = (int)(this.LastTouchPoint.X / (this.Bounds.Width / this.Items.Count));
        this.Items[index].IsSelected = !this.Items[index].IsSelected;
    }

    protected override void OnMoveHoverInteraction(object sender, TouchEventArgs e)
    {
        this.LastTouchPoint = e.Touches[0];
        this.ChangeVisualState();
    }

    internal void OnSelectedItemChanged(SegmentedItem item)
    {
        if (item.IsSelected && !this.MultiSelectMode)
        {
            foreach (var _item in this.Items)
            {
                if (!_item.Equals(item))
                {
                    _item.IsSelected = false;
                }
            }
        }

        this.SelectedItemsChanged?.Invoke(this, new(this.SelectedItems));

        if (this.Command?.CanExecute(this.CommandParameter ?? this.SelectedItems) is true)
            this.Command?.Execute(this.CommandParameter ?? this.SelectedItems);
    }

    protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    {
        var maxWidth = Math.Min(
            Math.Min(widthConstraint, this.MaximumWidthRequest),
            this.WidthRequest != -1 ? this.WidthRequest : double.PositiveInfinity
        );
        var maxHeight = Math.Min(
            Math.Min(heightConstraint, this.MaximumHeightRequest),
            this.HeightRequest != -1 ? this.HeightRequest : double.PositiveInfinity
        );

        var scale =
            (this.HeightRequest != -1 ? this.HeightRequest : Math.Min(40f, maxHeight)) / 40f;

        var maxItemWidth = 0d;
        var needHeight = 40f * scale;

        foreach (var item in this.Items)
        {
            var iconSize = 18f * scale;
            var textSize = this.GetStringSize(item.Text);
            //16 + iconSize + 8 + textSize.Width + 16
            maxItemWidth =
                textSize == SizeF.Zero
                    ? Math.Max(maxItemWidth, iconSize + iconSize + (16f + 8f + 16f) * scale)
                    : Math.Max(maxItemWidth, iconSize + 16f + textSize.Width + (8f + 16f) * scale);
        }

        var width =
            this.HorizontalOptions.Alignment == LayoutAlignment.Fill
                ? maxWidth
                : this.Margin.HorizontalThickness
                    + Math.Max(
                        this.MinimumWidthRequest,
                        this.WidthRequest == -1
                            ? Math.Min(maxWidth, maxItemWidth * this.Items.Count)
                            : this.WidthRequest
                    );
        var height =
            this.VerticalOptions.Alignment == LayoutAlignment.Fill
                ? maxHeight
                : this.Margin.VerticalThickness
                    + Math.Max(
                        this.MinimumHeightRequest,
                        this.HeightRequest == -1
                            ? Math.Min(maxHeight, needHeight)
                            : this.HeightRequest
                    );

        this.DesiredSize = new Size(Math.Ceiling(width), Math.Ceiling(height));
        return this.DesiredSize;
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() => this.Items.ToList();

    protected override void Dispose(bool disposing)
    {
        if (!this.disposedValue && disposing)
        {
            this.Clicked -= this.OnClicked;
            this.MoveHoverInteraction -= this.OnMoveHoverInteraction;
            this.Items.CollectionChanged -= (
                (IItemsElement<SegmentedItem>)this
            ).OnItemsCollectionChanged;
            if (this.ItemsSource is INotifyCollectionChanged ncc)
            {
                ncc.CollectionChanged -= ((IItemsSourceElement)this).OnItemsSourceCollectionChanged;
            }
            ((IIconElement)this).IconPath?.Dispose();
        }
        base.Dispose(disposing);
    }
}
