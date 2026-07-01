namespace RoverTest.ModelUserInterface
{
    public interface ITable : IElement
    {

        public IElement GetRow(int rowIndex);

        public IElement GetCell(IElement row, int columnIndex);

        public IElements GetCellsFromColumn(int columnIndex);
    }
}


