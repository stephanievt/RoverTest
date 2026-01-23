using RoverTest.ModelApplicationData;
using RoverTest.ModelUserInterface;

namespace RoverTest
{
    //DESIGN: Seriously I don't know what to call this. A word is a word.
    // I can't do this directly in implementation. I have stuff I want to override.
    // So make this ROVER a something that is implemented in 
    // the model.
    // For now, since it is shorter, I am calling it ecm for 
    public abstract class RoverEcm
    {
        // This is kinda arbitrary. I don't know how to think of these things yet.
        // "DRIVER" vs feature file vs whatever... probably overthinking.
        // But what IS this rover test guy. He is a single unit of "test"
        // whatever the consumer (at the moment me) sees it as.
        public string Name { get; set; }

        // I think this is going to tell me more about what it is soon.
        public List<RoverPageStack> AccessedPageStack { get; set; } = [];

        public List<RoverObjectCollection> RoverObjectCollections = [];

        protected RoverEcm(AppDriver appDriver)
        {
            AppDriver = appDriver;
            // ReSharper disable once VirtualMemberCallInConstructor
            LoadRoverDataObjects();
        }

        public AppDriver AppDriver { get; private set; }

        public abstract void LoadRoverDataObjects();
    }
}
