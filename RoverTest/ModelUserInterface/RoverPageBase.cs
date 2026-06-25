using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace RoverTest.ModelUserInterface
{
    /// <summary>
    /// This abstract class serves as CONTRACT and service provider
    /// to the consumer of the rover framework. Each page of the application
    /// implements RoverPageBase.
    /// Rover pages are created ONLY by factory.
    /// App driver property will be set in factory.
    /// </summary>
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members",
    Justification = "Derived classes use fields initialized via reflection")]
    public abstract class RoverPageBase
    {
        private readonly AppDriver _appDriver;

        public RoverPageAction LastRoverPageAction { get; set; }

        protected RoverPageBase(AppDriver appDriver)
        {
            _appDriver = appDriver;
            RegisterRoverPageActions();
        }

        public List<RoverPageAction> AvailableRoverPageActions { get; set; } = [];

        public List<RoverPageAction> AccessedRoverPageActionsStack { get; set; } = [];

        public abstract IElement PageIdentifier { get; }

        public abstract bool Exists { get; }

        /// <summary>
        /// Gets the AppDriver for use in derived classes
        /// </summary>
        protected AppDriver AppDriver => _appDriver;


        public void RegisterRoverPageActions()
        {
            Type derivedType = GetType();
            MethodInfo[] allMethods = derivedType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (var method in allMethods)
            {
                var attribute = method.GetCustomAttribute<RoverPageActionMethodAttribute>();

                if (attribute != null)
                {
                    if (attribute.RoverPageName == derivedType.Name)
                    {
                        RoverPageAction pa = new RoverPageAction
                        {
                            MethodName = attribute.RoverMethodName,
                            ResultRoverPage = this
                        };

                        AvailableRoverPageActions.Add(pa);
                    }
                }
            }
        }

        public void ExecuteRoverPageAction(string actionName)
        {
            RoverPageAction action = AvailableRoverPageActions
                .FirstOrDefault(a => a.MethodName == actionName);

            if (action != null)
            {
                var method = GetType().GetMethod(action.MethodName);
                if (method != null)
                {
                    method.Invoke(this, null);
                    AccessedRoverPageActionsStack.Add(action);
                    LastRoverPageAction = action;
                }
            }
            else
            {
                throw new InvalidOperationException($"Action '{actionName}' not found on page {GetType().Name}");
            }
        }

        /// <summary>
        /// Takes a screenshot of the current page
        /// </summary>
        /// <returns>Screenshot as byte array (PNG format)</returns>
        public byte[] TakeScreenshot()
        {
            return AppDriver.TakeScreenshot();
        }

        /// <summary>
        /// Takes a screenshot and saves it to a file
        /// </summary>
        /// <param name="filePath">Full path where the screenshot should be saved</param>
        public void TakeScreenshot(string filePath)
        {
            var screenshotBytes = AppDriver.TakeScreenshot();

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllBytes(filePath, screenshotBytes);
        }

        /// <summary>
        /// Takes a screenshot and returns as base64 string for embedding in HTML reports
        /// </summary>
        /// <returns>Base64 encoded PNG image</returns>
        public string TakeScreenshotAsBase64()
        {
            var screenshotBytes = AppDriver.TakeScreenshot();
            return Convert.ToBase64String(screenshotBytes);
        }
    }
}