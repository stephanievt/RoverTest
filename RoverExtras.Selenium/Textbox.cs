using System.Runtime;
using OpenQA.Selenium;
using RoverTest.ModelUserInterface;

namespace RoverExtras.Selenium;

public class Textbox(SeleniumAppDriver appDriver, By by) : Element(appDriver, by), ITextbox
{
    public void SendKeys(string entryValue)
    {
        if (string.IsNullOrWhiteSpace(entryValue))
            throw new AmbiguousImplementationException("No value sent to send keys. No keys to send.");
        WebElement.Clear();
        WebElement.SendKeys(entryValue);
    }
}