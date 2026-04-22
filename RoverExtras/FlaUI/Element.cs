using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using RoverTest.ModelUserInterface;

namespace RoverExtras.FlaUI
{

    //FlaUI has a neat set of elements. Add as we go.
    public class Element : IElement
    {
        private static ConditionFactory cf = new ConditionFactory(new UIA3PropertyLibrary());
        private readonly AutomationElement _automationElement;

        public Element(FlaUIAppDriver flauiAppDriver)
        {
            PropertyCondition condition = cf.ByAutomationId("40");
            AutomationElement foundElement = flauiAppDriver.MainWindow.FindFirstDescendant(condition);
            _automationElement = foundElement;
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

        
        
    }
}
