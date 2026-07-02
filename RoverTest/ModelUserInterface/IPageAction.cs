namespace RoverTest.ModelUserInterface
{
    public interface IPageAction
    {
        string Name { get; }

        void Execute();

        bool Pass { get; }

        string ReportableDetails { get; }
    }
}
