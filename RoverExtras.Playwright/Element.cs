using Microsoft.Playwright;
using RoverTest.ModelUserInterface;

namespace RoverExtras.Playwright
{
    public class Element : IElement
    {
        private readonly PlaywrightAppDriver _driver;
        private ILocator _locator;
        private readonly IPage _page;
        private readonly LocatorType? _locatorType;
        private readonly string _locatorString;

        public Element(PlaywrightAppDriver driver, LocatorType locatorType, string locatorString)
        {
            _driver = driver;
            _page = (IPage)_driver.Driver;
            _locatorType = locatorType;
            _locatorString = locatorString;
        }

        // Internal constructor for finding multiple elements from a parent element
        internal Element(PlaywrightAppDriver appDriver, ILocator locator)
        {
            _driver = appDriver;
            _page = (IPage)_driver.Driver;
            _locator = locator;
        }

        // expose this to Elements
        internal PlaywrightAppDriver PlaywrightAppDriver => _driver;

        // This is playwright specific so that 
        // elements that inherit can locate as 
        // with table.
        public IPage Page => _page;

        protected internal ILocator Locator
        {
            get
            {
                if (_locator == null)
                {
                    _locator = CreateLocator();
                }
                return _locator;
            }
        }

        private ILocator CreateLocator()
        {
            if (_locatorType == null)
            {
                throw new InvalidOperationException("Cannot create locator without a locator type");
            }

            return _locatorType switch
            {
                LocatorType.Role => _page.GetByRole(ParseRole(_locatorString)),
                LocatorType.Text => _page.GetByText(_locatorString),
                LocatorType.Label => _page.GetByLabel(_locatorString),
                LocatorType.Placeholder => _page.GetByPlaceholder(_locatorString),
                LocatorType.AltText => _page.GetByAltText(_locatorString),
                LocatorType.Title => _page.GetByTitle(_locatorString),
                LocatorType.TestId => _page.GetByTestId(_locatorString),
                LocatorType.LocatorMethod => _page.Locator(_locatorString),
                _ => throw new ArgumentException($"Unsupported locator type: {_locatorType}")
            };
        }

        private AriaRole ParseRole(string roleString)
        {
            // Handle role strings like "'heading', { name: 'Companies' }"
            var parts = roleString.Split(',');
            var roleName = parts[0].Trim().Trim('\'', '"');

            return roleName.ToLowerInvariant() switch
            {
                "heading" => AriaRole.Heading,
                "button" => AriaRole.Button,
                "link" => AriaRole.Link,
                "textbox" => AriaRole.Textbox,
                "checkbox" => AriaRole.Checkbox,
                "radio" => AriaRole.Radio,
                "combobox" => AriaRole.Combobox,
                "listbox" => AriaRole.Listbox,
                "menu" => AriaRole.Menu,
                "menuitem" => AriaRole.Menuitem,
                "tab" => AriaRole.Tab,
                "tabpanel" => AriaRole.Tabpanel,
                _ => throw new ArgumentException($"Unsupported role: {roleName}")
            };
        }

        public bool Visible
        {
            get
            {
                try
                {
                    return Locator.IsVisibleAsync().GetAwaiter().GetResult();
                }
                catch
                {
                    return false;
                }
            }
        }

        public void Click()
        {
            Locator.ClickAsync().GetAwaiter().GetResult();
        }

        public void Highlight()
        {
            // Playwright highlighting using evaluate
            Locator.EvaluateAsync("element => { element.style.border='2px solid red'; element.style.background='yellow'; }").GetAwaiter().GetResult();
        }

        public string Text => Locator.TextContentAsync().GetAwaiter().GetResult() ?? string.Empty;

        /// <summary>
        /// Waits for the element to be visible and enabled (editable).
        /// </summary>
        /// <param name="timeoutMs">Maximum time to wait in milliseconds. Default is 5000.</param>
        protected void WaitUntilReady(float timeoutMs = 5000)
        {
            Locator.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = timeoutMs
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Waits for the element to be in the specified state.
        /// </summary>
        /// <param name="state">The state to wait for.</param>
        /// <param name="timeoutMs">Maximum time to wait in milliseconds. Default is 5000.</param>
        protected void WaitForState(WaitForSelectorState state, float timeoutMs = 5000)
        {
            Locator.WaitForAsync(new LocatorWaitForOptions
            {
                State = state,
                Timeout = timeoutMs
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Waits for the element to be editable (enabled and not readonly).
        /// </summary>
        /// <param name="timeoutMs">Maximum time to wait in milliseconds. Default is 5000.</param>
        protected void WaitUntilEditable(float timeoutMs = 5000)
        {
            Locator.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = timeoutMs
            }).GetAwaiter().GetResult();

            // Playwright's IsEditableAsync ensures element is enabled and not readonly
            var deadline = DateTime.Now.AddMilliseconds(timeoutMs);
            while (DateTime.Now < deadline)
            {
                if (Locator.IsEditableAsync().GetAwaiter().GetResult())
                {
                    return;
                }
                Task.Delay(100).GetAwaiter().GetResult();
            }
            throw new TimeoutException($"Element did not become editable within {timeoutMs}ms");
        }
 
    }
}