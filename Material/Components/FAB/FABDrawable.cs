namespace Material;

internal class FABDrawable(FAB view) : IDrawable
{
    void IDrawable.Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(view.GetClipPath(rect));

        canvas.DrawBackground(view, rect);
        canvas.DrawOverlayLayer(view, rect);

        for (var rippleIndex = 0; rippleIndex < view.Ripples.Count; rippleIndex++)
        {
            canvas.DrawRipple(
                view,
                view.LastTouchPoint,
                view.Ripples[rippleIndex]
            );
        }

        var scale = rect.Height / 56f;
        canvas.DrawIcon(
            view,
            new RectF(16f * scale, 16f * scale, 24f * scale, 24f * scale),
            24,
            scale
        );
        canvas.ResetState();
    }
}
