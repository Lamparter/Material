﻿namespace Material.FluentExtensions;

public static class IIconElementExtension
{
    public static TBindable IconData<TBindable>(this TBindable view, string value)
        where TBindable : BindableObject, IIconElement
    {
        view.SetValue(IIconElement.IconDataProperty, value);
        return view;
    }

    public static TBindable BindIconData<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, IIconElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IIconElement.IconDataProperty, binding);
        return view;
    }

    public static TBindable IconColor<TBindable>(this TBindable view, Color value)
        where TBindable : BindableObject, IIconElement
    {
        view.SetValue(IIconElement.IconColorProperty, value);
        return view;
    }

    public static TBindable BindIconColor<TBindable>(
        this TBindable view,
        string path,
        BindingMode mode = BindingMode.Default,
        IValueConverter converter = null,
        object converterParameter = null,
        string stringFormat = null,
        object source = null
    ) where TBindable : BindableObject, IIconElement
    {
        var binding = new Binding(path, mode, converter, converterParameter, stringFormat, source);
        view.SetBinding(IIconElement.IconColorProperty, binding);
        return view;
    }
}
