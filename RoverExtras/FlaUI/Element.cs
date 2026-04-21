using FlaUI.Core.AutomationElements;
using RoverTest.ModelUserInterface;

namespace RoverExtras.FlaUI
{

    //FlaUI has a neat set of elements. Add as we go.

    public class Element : IElement
    {

        private readonly AutomationElement _automationElement;

        public Element(AutomationElement automationElement)
        {
            _automationElement = automationElement;
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
