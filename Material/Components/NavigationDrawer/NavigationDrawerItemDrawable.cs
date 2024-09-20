namespace Material;
internal class NavigationDrawerItemDrawable(NavigationDrawerItem view) : IDrawable
{
    private readonly NavigationDrawerItem view = view;

    public void Draw(ICanvas canvas, RectF rect)
    {
        if (rect == RectF.Zero) return;

        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(this.view.GetClipPath(rect));

        canvas.DrawBackground(this.view, rect);

        for (var rippleIndex = 0; rippleIndex < view.Ripples.Count; rippleIndex++)
        {
            canvas.DrawRipple(
                view,
                view.LastTouchPoint,
                view.Ripples[rippleIndex]
            );
        }

        canvas.DrawIcon(
           this.view,
           new RectF(24f, 16f, 24f, 24f),
           24,
           1f
       );

        canvas.DrawText(this.view, new RectF(60f, 0, rect.Width - 40f, rect.Height), HorizontalAlignment.Left);
        canvas.ResetState();
    }
}
