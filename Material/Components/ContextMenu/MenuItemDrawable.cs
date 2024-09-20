namespace Material;

internal class MenuItemDrawable(MenuItem view) : IDrawable
{
    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipRectangle(rect);
        
        for (var rippleIndex = 0; rippleIndex < view.Ripples.Count; rippleIndex++)
        {
            canvas.DrawRipple(
                view,
                view.LastTouchPoint,
                view.Ripples[rippleIndex]
            );
        }
        canvas.DrawIcon(view, new RectF(12f, 12f, 24f, 24f), 24, 1f);

        var iconSize = !string.IsNullOrEmpty(view.IconData) ? 24f : 0f;
        canvas.DrawText(
            view,
            new RectF(12f + iconSize + 12f, 0, rect.Width - iconSize, rect.Height),
            HorizontalAlignment.Left,
            VerticalAlignment.Center
        );

        if (!string.IsNullOrEmpty(view.TrailingIconData))
        {
            canvas.FillColor = view.TrailingIconColor.WithAlpha(
                view.ViewState is ViewState.Disabled ? 0.38f : 1f
            );

            using var path = new PathF((view as ITrailingIconElement).TrailingIconPath);
            var sx = rect.Right - 12f - 24f;
            var sy = rect.Center.Y - 12f;
            path.Move(sx, sy);
            canvas.FillPath(path);
        }

        canvas.ResetState();
    }
}
