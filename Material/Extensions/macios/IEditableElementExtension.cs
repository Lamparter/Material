﻿namespace Material.Extensions.Platform;

internal static class IEditableElementExtension
{
    public static SizeF GetLayoutSize<TElement>(this TElement element, float maxWidth)
        where TElement : IEditableElement, IFontElement
    {
        throw new NotImplementedException();
    }

    public static CaretInfo GetCaretInfo<TElement>(
        this TElement element,
        float maxWidth,
        PointF point
    ) where TElement : IEditableElement, IFontElement
    {
        throw new NotImplementedException();
    }

    public static CaretInfo GetCaretInfo<TElement>(
        this TElement element,
        float maxWidth,
        int location
    ) where TElement : IEditableElement, IFontElement
    {
        throw new NotImplementedException();
    }

    public static (RectF, RectF) GetSelectionRect<TElement>(this TElement element, float maxWidth)
        where TElement : IEditableElement, IFontElement
    {
        throw new NotImplementedException();
    }

    public static bool ShowKeyboard(this TextField editor)
    {
        throw new NotImplementedException();
    }

    public static bool CheckKeyboard(this TextField editor)
    {
        throw new NotImplementedException();
    }

    public static bool HideKeyboard(this TextField editor)
    {
        throw new NotImplementedException();
    }
}
