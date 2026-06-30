namespace RoverExtras.Playwright.PlaywrightAttributes
{
    /// <summary>
    /// Attribute to specify how an element should be located using Playwright-style locators.
    /// Automatically initializes IElement properties during page construction.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LocateByAttribute : Attribute
    {
        /// <summary>
        /// The type of locator to use (Role, Text, Label, etc.)
        /// </summary>
        public LocatorType How { get; set; }

        /// <summary>
        /// The locator string value
        /// </summary>
        public string Using { get; set; }

        public LocateByAttribute()
        {
        }

        public LocateByAttribute(LocatorType how, string @using)
        {
            How = how;
            Using = @using;
        }
    }
}