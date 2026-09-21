namespace MauiAppTempoAgora;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(
        IActivationState? activationState)
    {
        Window window = new Window(new MainPage());

        window.Width = 400;
        window.Height = 500;

        return window;
    }
}