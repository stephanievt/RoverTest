namespace RoverTest.ModelUserInterface
{
    public interface IPageAction
    {
        string Name { get; }

        bool Execute();
    }
}
