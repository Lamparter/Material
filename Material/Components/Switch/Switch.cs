﻿using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Animations;

namespace Material;

public class Switch
    : GraphicsView,
        IIconElement,
        IOutlineElement,
        IElement,
        IBackgroundElement,
        IShapeElement,
        IStateLayerElement,
        IICommandElement,
        IVisualTreeElement,
        IStyleElement,
        IDisposable
{
    protected bool IsVisualStateChanging { get; set; }
    ViewState viewState = ViewState.Normal;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ViewState ViewState
    {
        get => this.viewState;
        set
        {
            this.viewState = value;
            this.ChangeVisualState();
        }
    }

    public void OnPropertyChanged()
    {
        if (this.Handler != null && !this.IsVisualStateChanging)
        {
            this.Invalidate();
        }
    }

    void IElement.InvalidateMeasure()
    {
        this.InvalidateMeasure();
    }

    protected override void ChangeVisualState()
    {
        this.IsVisualStateChanging = true;
        var state = this.ViewState switch
        {
            ViewState.Normal => this.IsSelected ? "selected_normal" : "unselected_normal",
            ViewState.Hovered => this.IsSelected ? "selected_hovered" : "unselected_hovered",
            ViewState.Pressed => this.IsSelected ? "selected_pressed" : "unselected_pressed",
            ViewState.Disabled => this.IsSelected ? "selected_disabled" : "unselected_disabled",
            _ => "unselected_normal",
        };
        VisualStateManager.GoToState(this, state);
        this.IsVisualStateChanging = false;

        this.Invalidate();
    }

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected),
        typeof(bool),
        typeof(Switch),
        false,
        propertyChanged: (bo, ov, nv) =>
        {
            var view = bo as Switch;
            view.ChangeVisualState();
            view.SelectedChanged?.Invoke(view, new((bool)nv));

            if (view.Command?.CanExecute(view.CommandParameter ?? nv) is true)
                view.Command?.Execute(view.CommandParameter ?? nv);
        }
    );

    public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(
        nameof(ThumbColor),
        typeof(Color),
        typeof(Switch),
        propertyChanged: (bo, ov, nv) => ((Switch)bo).OnPropertyChanged()
    );
    public static new readonly BindableProperty IsEnabledProperty = IElement.IsEnabledProperty;
    public static new readonly BindableProperty BackgroundColorProperty =
        IBackgroundElement.BackgroundColorProperty;
    public static readonly BindableProperty OutlineWidthProperty =
        IOutlineElement.OutlineWidthProperty;
    public static readonly BindableProperty OutlineColorProperty =
        IOutlineElement.OutlineColorProperty;
    public static readonly BindableProperty ShapeProperty = IShapeElement.ShapeProperty;
    public static readonly BindableProperty IconDataProperty = IIconElement.IconDataProperty;
    public static readonly BindableProperty IconColorProperty = IIconElement.IconColorProperty;
    public static readonly BindableProperty StateLayerColorProperty =
        IStateLayerElement.StateLayerColorProperty;

    public static readonly BindableProperty CommandProperty = IICommandElement.CommandProperty;
    public static readonly BindableProperty CommandParameterProperty =
        IICommandElement.CommandParameterProperty;

    public static readonly BindableProperty DynamicStyleProperty =
        IStyleElement.DynamicStyleProperty;

    public string DynamicStyle
    {
        get => (string)this.GetValue(DynamicStyleProperty);
        set => this.SetValue(DynamicStyleProperty, value);
    }

    public Color ThumbColor
    {
        get => (Color)this.GetValue(ThumbColorProperty);
        set => this.SetValue(ThumbColorProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)this.GetValue(IsSelectedProperty);
        set => this.SetValue(IsSelectedProperty, value);
    }

    public new bool IsEnabled
    {
        get => (bool)this.GetValue(TouchGraphicsView.IsEnabledProperty);
        set => this.SetValue(TouchGraphicsView.IsEnabledProperty, value);
    }

    public new Color BackgroundColor
    {
        get => (Color)this.GetValue(BackgroundColorProperty);
        set => this.SetValue(BackgroundColorProperty, value);
    }

    [TypeConverter(typeof(ShapeConverter))]
    public Shape Shape
    {
        get => (Shape)this.GetValue(ShapeProperty);
        set => this.SetValue(ShapeProperty, value);
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
    public string IconData
    {
        get => (string)this.GetValue(IconDataProperty);
        set => this.SetValue(IconDataProperty, value);
    }

    PathF IIconElement.IconPath { get; set; }

    public Color IconColor
    {
        get => (Color)this.GetValue(IconColorProperty);
        set => this.SetValue(IconColorProperty, value);
    }

    public Color StateLayerColor
    {
        get => (Color)this.GetValue(StateLayerColorProperty);
        set => this.SetValue(StateLayerColorProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)this.GetValue(CommandProperty);
        set => this.SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => this.GetValue(CommandParameterProperty);
        set => this.SetValue(CommandParameterProperty, value);
    }

    public event EventHandler<CheckedChangedEventArgs> SelectedChanged;

    protected IAnimationManager animationManager;
    bool isTouching = false;

    public Switch()
    {
        this.SetDynamicResource(StyleProperty, "DefaultSwitchStyle");

        this.Drawable = new SwitchDrawable(this);
        this.StartInteraction += this.OnStartInteraction;
        this.EndInteraction += this.OnEndInteraction;
        this.CancelInteraction += this.OnCancelInteraction;
        this.StartHoverInteraction += this.OnStartHoverInteraction;
        this.EndHoverInteraction += this.OnEndHoverInteraction;
    }

    private void OnStartInteraction(object sender, TouchEventArgs e)
    {
        if (!this.IsEnabled)
            return;

        this.ViewState = ViewState.Pressed;
        this.isTouching = true;
    }

    private void OnEndInteraction(object sender, TouchEventArgs e)
    {
        if (!this.IsEnabled)
            return;

        if (this.isTouching)
            this.IsSelected = !this.IsSelected;

#if __MOBILE__
        this.ViewState = ViewState.Normal;
#else
        if (e.IsInsideBounds)
        {
            this.ViewState = e.IsInsideBounds ? ViewState.Hovered : ViewState.Normal;
        }
#endif

        this.isTouching = false;
    }

    private void OnCancelInteraction(object sender, EventArgs e)
    {
        this.isTouching = false;
    }

    private void OnStartHoverInteraction(object sender, TouchEventArgs e)
    {
        if (!this.IsEnabled)
            return;

        this.ViewState = ViewState.Hovered;
    }

    private void OnEndHoverInteraction(object sender, EventArgs e)
    {
        if (!this.IsEnabled)
            return;

        this.ViewState = ViewState.Normal;
    }

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                this.StartInteraction -= this.OnStartInteraction;
                this.EndInteraction -= this.OnEndInteraction;
                this.CancelInteraction -= this.OnCancelInteraction;
                this.StartHoverInteraction -= this.OnStartHoverInteraction;
                this.EndHoverInteraction -= this.OnEndHoverInteraction;
            }
            this.disposedValue = true;
        }
    }

    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
