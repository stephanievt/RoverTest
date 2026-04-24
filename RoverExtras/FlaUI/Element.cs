using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using RoverTest.ModelUserInterface;
using System.Reflection;

namespace RoverExtras.FlaUI
{

    //FlaUI has a neat set of elements. Add as we go.
    public class Element : IElement
    {
        private static ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());
        private readonly AutomationElement _automationElement;

        public Element(FlaUIAppDriver flauiAppDriver)
        {
            _automationElement = FindElement(flauiAppDriver);
        }

        public bool Visible => _automationElement?.IsAvailable == true &&
                               !_automationElement.IsOffscreen &&
                               !_automationElement.BoundingRectangle.IsEmpty;

        public void Click()
        {
            _automationElement?.WaitUntilClickable();
            _automationElement?.Click();
        }

        public void Highlight()
        {
            _automationElement.DrawHighlight();
        }

        public string Text => _automationElement.AsLabel()?.Text ?? string.Empty;

        private AutomationElement FindElement(FlaUIAppDriver flauiAppDriver)
        {
            // Get the stack trace to find the calling property
            var stackTrace = new System.Diagnostics.StackTrace();

            // Walk up the stack to find the property that has the FlaUIFinder attribute
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var method = stackTrace.GetFrame(i)?.GetMethod();
                if (method == null) continue;

                // Check if this is a property getter
                if (method.Name.StartsWith("get_"))
                {
                    var propertyName = method.Name.Substring(4); // Remove "get_" prefix
                    var declaringType = method.DeclaringType;

                    if (declaringType != null)
                    {
                        var property = declaringType.GetProperty(propertyName,
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

                        if (property != null)
                        {
                            var finderAttr = property.GetCustomAttribute<FlaUIFinder>();
                            if (finderAttr != null)
                            {
                                return FindElementByAttribute(flauiAppDriver, finderAttr);
                            }
                        }
                    }
                }
            }

            // Fallback to default if no attribute found
            PropertyCondition condition = cf.ByAutomationId("40");
            return flauiAppDriver.MainWindow.FindFirstDescendant(condition);
        }

        private AutomationElement FindElementByAttribute(FlaUIAppDriver flauiAppDriver, FlaUIFinder finder)
        {
            ConditionBase condition = null;

            // Build condition based on SearchTech
            switch (finder.SearchTech)
            {
                case FlauiSearchTech.ByAutomationId:
                    condition = cf.ByAutomationId(finder.FinderString);
                    break;
                case FlauiSearchTech.ByName:
                    condition = cf.ByName(finder.FinderString);
                    break;
                case FlauiSearchTech.ByClassName:
                    condition = cf.ByClassName(finder.FinderString);
                    break;
                case FlauiSearchTech.ByXPath:
                    // XPath requires different handling
                    return flauiAppDriver.MainWindow.FindFirstByXPath(finder.FinderString);
                default:
                    condition = cf.ByAutomationId(finder.FinderString);
                    break;
            }

            return flauiAppDriver.MainWindow.FindFirstDescendant(condition);
        }


    }
}
