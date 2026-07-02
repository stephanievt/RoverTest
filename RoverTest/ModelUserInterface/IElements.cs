namespace RoverTest.ModelUserInterface
{
    public interface IElements : IEnumerable<IElement>
    {
        public int Count { get; }
    }
}
