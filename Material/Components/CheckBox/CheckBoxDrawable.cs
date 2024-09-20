namespace Material;

internal class CheckBoxDrawable(CheckBox view) : IDrawable
{
    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.Antialias = true;
        var scale = rect.Height / 40f;
        var drawRect = new RectF(11f * scale, 11f * scale, 18f * scale, 18f * scale);
        if (view.IsChecked)
        {
            canvas.SaveState();
            canvas.ClipPath(view.GetClipPath(drawRect));
            canvas.DrawBackground(view, drawRect);
            canvas.RestoreState();
        }

        this.DrawStateLayer(canvas, rect);

        if (view.IsChecked)
            canvas.DrawIcon(view, drawRect, 18, scale);
        else
            canvas.DrawOutline(view, drawRect);
    }

    void DrawStateLayer(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        var drawRect = new PathF();
        drawRect.AppendCircle(rect.Center.X, rect.Center.Y, Math.Max(rect.Width, rect.Height) / 2f);
        canvas.ClipPath(drawRect);

        for (var rippleIndex = 0; rippleIndex < view.Ripples.Count; rippleIndex++)
        {
            canvas.DrawRipple(
                view,
                view.LastTouchPoint,
                view.Ripples[rippleIndex]
            );
        }

        canvas.RestoreState();
    }
}
