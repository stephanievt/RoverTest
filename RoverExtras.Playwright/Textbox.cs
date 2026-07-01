using RoverTest.ModelUserInterface;

namespace RoverExtras.Playwright
{
    public class Textbox(PlaywrightAppDriver driver, LocatorType locatorType, string locatorString)
        : Element(driver, locatorType, locatorString), ITextbox
    {
        public void SendKeys(string entryValue)
        {
            Locator.FillAsync(entryValue).GetAwaiter().GetResult();
        }
    }
}
