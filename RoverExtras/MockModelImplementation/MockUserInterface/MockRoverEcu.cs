using RoverTest;
using RoverTest.ModelUserInterface;

namespace RoverExtras.MockModelImplementation.MockUserInterface;

public class MockRoverEcu(AppDriver appDriver) : RoverEcu(appDriver)
{
    public override void LoadRoverDataObjects()
    {
        throw new NotImplementedException();
    }

   
}