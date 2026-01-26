using System.Diagnostics;
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

    public abstract class RoverPageBase
    {
        public RoverPageAction LastRoverPageAction { get; set; }

        // ReSharper disable once VirtualMemberCallInConstructor
        protected RoverPageBase()
        {
            RegisterRoverPageActions();
        }

        public List<RoverPageAction> AvailableRoverPageActions { get; set; } = [];

        public List<RoverPageAction> AccessedRoverPageActionsStack { get; set; } = [];

        public AppDriver AppDriver => RoverEcu.AppDriver;

        public RoverEcu RoverEcu { get; set; }

        public abstract string Url { get; set; }

        public abstract IElement PageIdentifier { get; }

        public abstract bool Exists { get; }

        public abstract void Open();

        
        protected void RegisterWithRoverTest()
        {
            // get derived type 
            var thisType = this.GetType();
            RoverPageStack currentPageForStack = new RoverPageStack()
            {
                FullName = thisType.FullName,
                Name = thisType.Name,
                RoverPage = this
            };

            RoverEcu.AccessedPageStack.Add(currentPageForStack);
        }


        public void RegisterRoverPageActions()
        {
            Type derivedType = this.GetType();


            // Use GetMethods to get all methods (public instance methods by default)
            // You may need to specify BindingFlags for non-public, static, or other methods
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
                            MethodName = method.Name,
                            ResultRoverPage = this
                        };
                        AvailableRoverPageActions.Add(pa);
                    }

                }
            }

        }

        protected RoverPageAction StackPageAction(bool success)
        {
            // I want to find the CALLING method in the available list.
            StackTrace stackTrace = new StackTrace();
            StackFrame callingFrame = stackTrace.GetFrame(1);
            MethodBase method = callingFrame.GetMethod();

            RoverPageAction action = null;
            // Get RoverPageAction from list
            foreach (var roverPageAction in AvailableRoverPageActions)
            {
                if (roverPageAction.MethodName == method.Name)
                {
                    action = roverPageAction;
                    action.Result = success;
                    action.ResultRoverPage = this;
                    
                }
                
            }

            //OK so Now I have a RoverPageAction to add to RoverPage
            AccessedRoverPageActionsStack.Add(action);
            return action;
        }

        

        

    }
}
