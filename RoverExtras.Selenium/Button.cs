using OpenQA.Selenium;
using RoverTest.ModelUserInterface;

namespace RoverExtras.Selenium
{
    // There is no special need for button as of now.
    // However, it is more intuitive.
    // For test evidence, there is a possibility of a role for the button click
    // that demonstrates identifying buttons as a good idea from the beginning.
    // A button click is different from another element click for focus setting.
    //public class Button(AppDriver driver) : Element(driver), IButton
    public class Button(AppDriver appDriver, By by) : Element(appDriver, by), IButton;

    
}
