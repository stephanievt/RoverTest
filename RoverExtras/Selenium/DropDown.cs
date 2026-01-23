using OpenQA.Selenium;
using RoverTest.ModelUserInterface;

namespace RoverExtras.Selenium
{
    /// <summary>
    /// This is a drop-down implementation IN Selenium
    /// specifically for a FLEX implementation of a drop-down
    /// which is used in the app. The implementation of a drop-down
    /// is influenced by its implementation in the app.
    /// https://getbootstrap.com/docs/4.0/utilities/flex/
    /// </summary>
    /// <param name="appDriver"></param>
    /// <param name="by"></param>
    public class DropDown(AppDriver appDriver, By by) : Element(appDriver, by), IDropdown
    {
        private readonly AppDriver _appDriver = appDriver;

        // So my BY here refers to the element that HOLDS the other stuff. Currently 
        // avail through inheritance.
        // Items 
        public IElement Container => this;

        public IElement ClickForDrop => Container;

        public IElements Items
        {
            get
            {
                By childBy = By.TagName("a");
                Elements elements = new Elements(_appDriver, childBy, this);
                return elements;
            }
        }

        public void SelectItem(int id)
        {
            throw new NotImplementedException();
        }

        public void SelectItem(string itemName)
        {
            // This is the DROP click. 
            Click();
            foreach (var item in Items)
            {
                if (item.Text == itemName)
                {
                    item.Click();
                    break;
                }
            }
        }
    }
}
