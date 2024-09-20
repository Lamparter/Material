using RectF = Microsoft.Maui.Graphics.RectF;

namespace Material;

internal class ButtonDrawable(Button view) : IDrawable
{
    readonly Button view = view;

    public void Draw(ICanvas canvas, RectF rect)
    {
        canvas.SaveState();
        canvas.Antialias = true;
        canvas.ClipPath(this.view.GetClipPath(rect));

        canvas.DrawBackground(this.view, rect);
        canvas.DrawOutline(this.view, rect);
        canvas.DrawOverlayLayer(this.view, rect);

        for (var rippleIndex = 0; rippleIndex < this.view.Ripples.Count; rippleIndex++)
        {
            canvas.DrawRipple(
                this.view,
                this.view.LastTouchPoint,
                this.view.Ripples[rippleIndex]
            );
        }

        var scale = rect.Height / 40f;
        canvas.DrawIcon(
            this.view,
            new RectF(16f * scale, 11f * scale, 18f * scale, 18f * scale),
            18,
            scale
        );

        var iconSize = (!string.IsNullOrEmpty(this.view.IconData) ? 18f : 0f) * scale;
        canvas.DrawText(this.view, new RectF(iconSize, 0, rect.Width - iconSize, rect.Height));
        canvas.ResetState();
    }
}
