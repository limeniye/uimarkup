namespace UIMarkup.UI.UnoApp;

public static class AppBuilderExtensions
{
    public static MauiAppBuilder UseMauiControls(this MauiAppBuilder builder) =>
        builder
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("UIMarkup.UI.UnoApp/Assets/Fonts/OpenSansRegular.ttf", "OpenSansRegular");
                fonts.AddFont("UIMarkup.UI.UnoApp/Assets/Fonts/OpenSansSemibold.ttf", "OpenSansSemibold");
            });
}
