using Material.Handlers;

namespace Material.Extensions;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseMaterialComponents(this MauiAppBuilder builder)
    {
        return builder.ConfigureMauiHandlers(
            (handlers) =>
            {
                handlers.AddHandler(typeof(TextField), typeof(TextFieldHandler));
            }
        );
    }
}
