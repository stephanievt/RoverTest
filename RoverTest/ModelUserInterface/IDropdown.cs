namespace RoverTest.ModelUserInterface
{
    // Drop-downs are handled in app UIs in similar ways. In HTML,
    // you will see something (a ul or div or something) that contains 
    // the clickable thing that makes the list visible and the list itself.
    // With a SELECT tag, those are the same. In many HTML implementations, they are
    // the same, but are not always. In win forms apps even, they are not all std.
    public interface IDropdown : IElement
    {
        public IElement Container { get; }

        public IElement ClickForDrop { get; }

        public IElements Items { get; }

        public void SelectItem(int id);

        public void SelectItem(string itemName);
    }

    
}
