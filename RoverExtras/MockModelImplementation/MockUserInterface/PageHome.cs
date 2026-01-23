using RoverTest.ModelUserInterface;

namespace RoverExtras.MockModelImplementation.MockUserInterface
{
    [RoverPage]
    public class PageHome : RoverPageBase
    {
        public override string Url { get; set; }
        public override IElement PageIdentifier { get; } = new MockElement();
        public override bool Exists { get; } = true;

        

        public override void Open()
        {
            Console.WriteLine("PageHome Opened");
        }

        [RoverPageActionMethod("PageHome","SetName")]
        public RoverPageAction SetName(string firstName, string lastName)
        {
        
            
            RoverPageAction action = StackPageAction(true);
            return action;

        }
        
    }
}
