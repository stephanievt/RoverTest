using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RoverTest;
using RoverTest.ModelUserInterface;
using SeleniumExtras.WaitHelpers;

namespace RoverExtras.Selenium;

public class Element : IElement
{
    public readonly IWebElement WebElement;
    private readonly IWebDriver WebDriver;
    
    public By ByFinder { get; }

    public bool Exists { get; private set; }

    // For the ELEMENTS Collection ONLY
    internal Element(AppDriver appDriver, IWebElement webElement, By by)
    {
        ByFinder = by;
        WebDriver = RoverInternals.GetTypedObject<IWebDriver>(appDriver.Driver);
        WebElement = webElement;
        Exists = true;

    }
    public Element(AppDriver appDriver, By by, Element parent)
    {
        
        IWebElement webParent = parent.WebElement;
        ByFinder = by;
        WebDriver = RoverInternals.GetTypedObject<IWebDriver>(appDriver.Driver);

        WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30)); // Waits up to 10 seconds
        WebElement = wait.Until(ExpectedConditions.ElementToBeClickable(by));
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
        WebElement = wait.Until(ExpectedConditions.ElementToBeClickable(by));
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

    
}