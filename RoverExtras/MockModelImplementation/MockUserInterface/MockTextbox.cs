using RoverTest.ModelUserInterface;

namespace RoverExtras.MockModelImplementation.MockUserInterface
{
    public class MockTextbox : MockElement, ITextbox
    {
        public void SendKeys(string entryValue)
        {
            Console.WriteLine(className + " " + entryValue + " Keys sent");
        }
    }
}
