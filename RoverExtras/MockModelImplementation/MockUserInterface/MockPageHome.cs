//using RoverTest.ModelUserInterface;

//namespace RoverExtras.MockModelImplementation.MockUserInterface
//{
//    public class MockPageHome : RoverPageBase
//    {
//        public override IElement PageIdentifier { get; } = new MockElement();
//        public override bool Exists { get; } = true;

        
//        public MockTextbox SearchText { get; } = new MockTextbox();

//        public MockButton SearchButton { get; } = new MockButton();

//        public MockDropDown Categories { get; } = new MockDropDown();

//        public MockGrid ItemTable { get; } = new MockGrid();

//        [RoverPageActionMethod("PageHome", "SearchItems")]
//        public RoverPageAction SearchItems(string searchText)
//        {
//            SearchText.SendKeys(searchText);
//            SearchButton.Click();

//            //This is a MOCK
//            //in a real implementation, I would query the table.
//            //TODO: How do I want to mock this?
//            //This comes when I wire up BO linkage. The table 
//            // can contain rows and stuff.
//            RoverPageAction action = StackPageAction(true);
//            LastRoverPageAction = action;


//            return action;

//        }

//        [RoverPageActionMethod("PageHome", "SelectCategory")]
//        public RoverPageAction SelectCategory(int catId)
//        {
//            Categories.SelectItem(catId);
//            //TODO: How do I want to mock this?
//            RoverPageAction action = StackPageAction(true);
//            LastRoverPageAction = action;

//            return action;
//        }
//    }
//}
