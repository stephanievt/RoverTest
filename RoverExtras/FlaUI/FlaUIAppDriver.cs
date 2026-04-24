using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA2;
using FlaUI.UIA3;
using RoverTest.ModelUserInterface;

namespace RoverExtras.FlaUI
{
    public class FlaUIAppDriver : AppDriver
    {
        // These are 2 FlaUI specific objects
        // used to find on screen objects
        private readonly Application winApp;
        public Window FlaUiStartupWindow { get; }

        public FlaUIAppDriver(string location) : base(location)
        {
            winApp = Application.Launch(location);
            var automation = new UIA3Automation();
            //TODO: Fix this magic number.
            FlaUiStartupWindow = Retry.WhileNull(() => winApp.GetMainWindow(automation),
                timeout: TimeSpan.FromSeconds(120)).Result;
            Driver = winApp;
        }

        public override object Driver { get; }
        public override void Dispose()
        {
            winApp.Close();
            winApp.Dispose();
            
        }
    }
}
