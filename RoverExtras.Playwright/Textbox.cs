using RoverTest.ModelUserInterface;

namespace RoverExtras.Playwright
{
    public class Textbox(PlaywrightAppDriver driver, LocatorType locatorType, string locatorString)
        : Element(driver, locatorType, locatorString), ITextbox
    {
        public void SendKeys(string entryValue)
        {
            WaitUntilEditable();
            Locator.ClearAsync().GetAwaiter().GetResult();
            WaitUntilEditable();
            Locator.FillAsync(entryValue).GetAwaiter().GetResult();

        }
    }
}
