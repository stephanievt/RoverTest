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
        //TODO: This would make a great documentation example
        // to support the abstraction. Put a thread.sleep here
        // then show how you can fix it.
        WaitUntilReady();
        WebElement.SendKeys(entryValue);
        
    }
}