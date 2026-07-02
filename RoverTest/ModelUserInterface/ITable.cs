namespace RoverTest.ModelUserInterface
{
    public interface ITable : IElement
    {
        public IElement GetCell(int row, int columnIndex);

        public IElements GetCellsFromColumn(int colIndex);
    }
}


