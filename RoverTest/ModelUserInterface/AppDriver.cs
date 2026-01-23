namespace RoverTest.ModelUserInterface
{
    /// <summary>
    /// If I add something here, do I see it in the diagram?
    /// </summary>
    /// <param name="location"></param>
    public abstract class AppDriver(string location)
    {

        public abstract object Driver { get; set; }

        public string Location { get; private set; } = location;

        // This method allows implementation to clean up 
        public abstract void Dispose();
    }
}
