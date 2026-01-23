namespace RoverTest.ModelUserInterface;

public interface IElement
{
    public bool Visible { get; }
    public void Click();
    public void Highlight();

    public string Text { get; }

}