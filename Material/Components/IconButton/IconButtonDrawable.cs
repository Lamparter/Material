namespace Material;

class IconButtonDrawable(IconButton view) : IDrawable
{
    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(view.GetClipPath(rect));

        canvas.DrawBackground(view, rect);
        canvas.DrawOutline(view, rect);

        var scale = rect.Height / 40f;
        canvas.DrawIcon(view, rect, 24, scale);
        canvas.DrawOverlayLayer(view, rect);

        for (var rippleIndex = 0; rippleIndex < view.Ripples.Count; rippleIndex++)
        {
            canvas.DrawRipple(
                view,
                view.LastTouchPoint,
                view.Ripples[rippleIndex]
            );
        }

        canvas.ResetState();
    }
}
