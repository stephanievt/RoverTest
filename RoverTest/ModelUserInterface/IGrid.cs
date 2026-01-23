namespace RoverTest.ModelUserInterface
{
    public interface IGrid : IElement
    {
        public IElement GetRow(int rowIndex);

        public IElement GetCell(IElement row, int columnIndex);

    }
}


