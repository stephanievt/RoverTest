using RoverTest.ModelUserInterface;

namespace RoverExtras.MockModelImplementation.MockUserInterface
{
    public class MockElement : IElement
    {
        internal static string className;

        public MockElement()
        {
            className = GetType().FullName;
        }

        public bool Visible { get; } = true;
        public void Click()
        {
            
            Console.WriteLine(className + " Clicked");
        }

        public void Highlight()
        {
            
        }

        public string Text { get; set; } = className;
    }
}
