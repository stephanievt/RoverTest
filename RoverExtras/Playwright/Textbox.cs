using RoverTest.ModelUserInterface;

namespace RoverExtras.Playwright
{
    public class Textbox(PlaywrightAppDriver driver, LocatorType locatorType, string locatorString)
        : Element(driver, locatorType, locatorString), ITextbox
    {
        public async void SendKeys(string entryValue)
        {
            await Locator.FillAsync(entryValue);
        }
    }
}
