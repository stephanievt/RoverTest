namespace RoverTest.ModelUserInterface
{
    /// <summary>
    /// If I add something here, do I see it in the diagram?
    /// </summary>
    /// <param name="location"></param>
    public abstract class AppDriver(string location)
    {
        public abstract object Driver { get; }

        public string Location { get; private set; } = location;

        /// <summary>
        /// Navigates to the application location
        /// </summary>
        public abstract Task NavigateAsync();

        // This method allows implementation to clean up 
        public abstract void Dispose();

        public abstract byte[] TakeScreenshot();
    }
}
