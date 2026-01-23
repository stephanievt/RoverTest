using RoverTest.ModelUserInterface;

namespace RoverExtras.MockModelImplementation.MockUserInterface
{
    public  class MockDropDown : MockElement, IDropdown
    {
        public IElement Container { get; } = new MockElement();
        public IElement ClickForDrop { get; } = new MockElement();

        public IElements Items => throw new NotImplementedException();

        public void SelectItem(int id)
        {
            throw new NotImplementedException();
        }

        public void SelectItem(string itemName)
        {
            throw new NotImplementedException();
        }
    }
}
