namespace RoverTest.ModelUserInterface
{
    // The result page name (page obj) is how action methods know if they succeeded.
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RoverPageActionMethodAttribute(string roverResultPageName, string roverMethodName) : Attribute
    {
        public string RoverPageName { get; } = roverResultPageName;

        public string RoverMethodName { get; } = roverMethodName;
    }
}
