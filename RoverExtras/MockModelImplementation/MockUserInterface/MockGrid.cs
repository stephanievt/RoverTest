using RoverTest.ModelUserInterface;

namespace RoverExtras.MockModelImplementation.MockUserInterface
{
    public class MockGrid : MockElement, IGrid
    {
        public IElement GetRow(int rowIndex)
        {
            return new MockElement();
        }

        public IElement GetCell(IElement row, int columnIndex)
        {
            return new MockElement();
        }
    }
}
