using RoverTest.ModelUserInterface;

namespace RoverExtras.Playwright
{
    public class Button(PlaywrightAppDriver driver, LocatorType locatorType, string locatorString)
        : Element(driver, locatorType, locatorString), IButton;
}
