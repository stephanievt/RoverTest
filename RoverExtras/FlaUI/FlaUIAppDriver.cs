using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using RoverTest.ModelUserInterface;

namespace RoverExtras.FlaUI
{
    public class FlaUIAppDriver : AppDriver
    {
        private readonly Application winApp;
        
        // With FlaUI, we need the mainWindow 
        // to make element calls.
        public Window MainWindow { get; }

        public FlaUIAppDriver(string location) : base(location)
        {
            winApp = Application.Launch(location);
            //TODO: Implement waits correctly. This is a total hack.
            Thread.Sleep(10000);
            MainWindow = winApp.GetMainWindow(new UIA3Automation());
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
