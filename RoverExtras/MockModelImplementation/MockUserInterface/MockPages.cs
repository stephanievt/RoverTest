using RoverTest.ModelUserInterface;

namespace RoverExtras.MockModelImplementation.MockUserInterface
{
    public class MockPages : RoverPages
    {
        public override RoverPageBase GetPage(string pageName)
        {
            throw new NotImplementedException();
        }
    }
}
