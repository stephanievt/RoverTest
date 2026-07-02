using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RoverTest;
using RoverTest.ModelUserInterface;

namespace RoverExtras.Selenium;

public class Element : IElement
{
    public readonly IWebElement WebElement;
    private readonly IWebDriver WebDriver;
    
    public By ByFinder { get; }

    public bool Exists { get; private set; }

    public Element(AppDriver appDriver, By by, Element parent)
    {
        
        IWebElement webParent = parent.WebElement;
        ByFinder = by;
        WebDriver = RoverInternals.GetTypedObject<IWebDriver>(appDriver.Driver);

        WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30)); // Waits up to 10 seconds
        WebElement= wait.Until(drv =>
        {
            var el = drv.FindElement(by);
            return el.Displayed && el.Enabled ? el : null;
        });
        try
        {
            WebElement = webParent.FindElement(by);
            Exists = true;
        }
        catch (NoSuchElementException)
        {
            Exists = false;
        }
    }

    public Element(AppDriver appDriver, By by) 
    {
        ByFinder = by;
        WebDriver = RoverInternals.GetTypedObject<IWebDriver>(appDriver.Driver);

        WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30)); // Waits up to 10 seconds
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
        WebElement = wait.Until(drv =>
        {
            try
            {
                var el = drv.FindElement(by);
                return el.Displayed && el.Enabled ? el : null;
            }
            catch (StaleElementReferenceException)
            {
                return null; // Will retry
            }
        });
        try
        {
            WebElement = WebDriver.FindElement(by);
            Exists = true;
        }
        catch (NoSuchElementException)
        {
            Exists = false;
        }
        
    }

    internal Element(AppDriver appDriver, IWebElement webElement)
    {
        WebElement = webElement;
        WebDriver = RoverInternals.GetTypedObject<IWebDriver>(appDriver.Driver);
        Exists = true;
    }

    public void Click()
    {
        WebElement.Click();
        //TODO: This is not entirely right. But the click on an element causes an action.
        // A table or whatever cannot report itself properly. This may go elsewhere
        // really replaced with the right thing when 
        // remembering selenium timing shit.
        Thread.Sleep(2000);
    }

    public void Highlight()
    {
        string highlightScript = "arguments[0].style.border='2px solid red'; arguments[0].style.background='yellow';";
        IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebDriver;
        jsExecutor.ExecuteScript(highlightScript, WebElement);
    }

    public string Text => WebElement.Text;


    public bool Visible => WebElement.Displayed;

    //*** START: New wait methods added to replace Thread.Sleep usage ***
    /// <summary>
    /// Waits for the element to be in a state ready for interaction (enabled and displayed).
    /// </summary>
    /// <param name="timeoutSeconds">Maximum time to wait in seconds. Default is 10.</param>
    protected void WaitUntilReady(int timeoutSeconds = 10)
    {
        WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(timeoutSeconds));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
        wait.Until(_ =>
        {
            try
            {
                return WebElement.Displayed && WebElement.Enabled;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        });
    }

    /// <summary>
    /// Waits for a custom condition to be true for this element.
    /// </summary>
    /// <param name="condition">The condition to wait for.</param>
    /// <param name="timeoutSeconds">Maximum time to wait in seconds. Default is 10.</param>
    protected void WaitUntil(Func<IWebElement, bool> condition, int timeoutSeconds = 10)
    {
        WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(timeoutSeconds));
        wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
        wait.Until(_ =>
        {
            try
            {
                return condition(WebElement);
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        });
    }
    //*** END: New wait methods added ***

}