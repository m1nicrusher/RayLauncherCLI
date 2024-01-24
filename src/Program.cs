using Terminal.Gui;

namespace AdvancedMinecraftLauncher
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Application.Run<ExampleWindow>();

            // Before the application exits, reset Terminal.Gui for clean shutdown
            Application.Shutdown();
        }
    }
}
