﻿using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Material;

public partial class ContextMenu
{
    private static readonly Microsoft.UI.Xaml.Style flyoutStyle;
    private Flyout container;
    private FrameworkElement platformAnchor;

    static ContextMenu()
    {
        flyoutStyle = new Microsoft.UI.Xaml.Style(typeof(FlyoutPresenter));
        flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(Control.MarginProperty, 0));
        flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(Control.PaddingProperty, 0));
        flyoutStyle.Setters.Add(new Microsoft.UI.Xaml.Setter(Control.BorderThicknessProperty, 0));
        flyoutStyle.Setters.Add(
            new Microsoft.UI.Xaml.Setter(
                Control.BackgroundProperty,
                Colors.Transparent.ToWindowsColor()
            )
        );
        flyoutStyle.Setters.Add(
            new Microsoft.UI.Xaml.Setter(FlyoutPresenter.IsDefaultShadowEnabledProperty, false)
        );
    }

    private void PlatformShow(View anchor)
    {
        var context = anchor.Handler.MauiContext;
        var platformContent = this.ToPlatform(context);
        if (this.container == null)
        {
            this.platformAnchor = anchor.ToPlatform(context);
            platformContent.Width = this.DesiredSize.Width;
            platformContent.Height = this.DesiredSize.Height;

            this.container = new Flyout
            {
                FlyoutPresenterStyle = flyoutStyle,
                LightDismissOverlayMode = LightDismissOverlayMode.Off,
                Content = this.ToPlatform(context),
            };
            this.container.Closed += this.OnClosed;
        }
        this.container.ShowAt(this.platformAnchor);
    }

    private void PlatformShow(View anchor, Point point)
    {
        var context = anchor.Handler.MauiContext;
        var platformContent = this.ToPlatform(context);
        if (this.container == null)
        {
            this.platformAnchor = anchor.ToPlatform(context);
            platformContent.Width = this.DesiredSize.Width;
            platformContent.Height = this.DesiredSize.Height;

            this.container = new Flyout
            {
                FlyoutPresenterStyle = flyoutStyle,
                LightDismissOverlayMode = LightDismissOverlayMode.Off,
                Content = platformContent,
            };

            this.container.Closed += this.OnClosed;
        }



        this.container.ShowAt(
            this.platformAnchor,
            new Microsoft.UI.Xaml.Controls.Primitives.FlyoutShowOptions
            {
                Placement = Microsoft
                    .UI
                    .Xaml
                    .Controls
                    .Primitives
                    .FlyoutPlacementMode
                    .RightEdgeAlignedTop,
                Position = new Windows.Foundation.Point(point.X, point.Y)
            }
        );
    }

    public void Close(object result = null)
    {
        this.Result = result;
        this.container.Hide();
    }

    public bool IsShowing() => this.container?.IsOpen ?? false;
}
