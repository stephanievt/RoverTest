using Microsoft.Playwright;
using RoverTest.ModelUserInterface;

namespace RoverExtras.Playwright
{
    public class Textbox(PlaywrightAppDriver driver, LocatorType locatorType, string locatorString)
        : Element(driver, locatorType, locatorString), ITextbox
    {
        public void SendKeys(string entryValue)
        {
            Locator.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 5000
            }).GetAwaiter().GetResult();
            //*** End wait for element ***

            //*** Clear existing value before filling ***
            Locator.ClearAsync().GetAwaiter().GetResult();
            //*** End clear ***

            // Fill the textbox with the new value
            Locator.FillAsync(entryValue).GetAwaiter().GetResult();

            //*** Verify the value was actually set ***
            var actualValue = Locator.InputValueAsync().GetAwaiter().GetResult();
            if (actualValue != entryValue)
            {
                throw new InvalidOperationException(
                    $"Failed to set textbox value. Expected: '{entryValue}', Actual: '{actualValue}'");
            }
        }
    }
}
