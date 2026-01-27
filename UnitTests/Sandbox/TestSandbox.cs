using NUnit.Framework;
using RoverExtras.MockModelImplementation.MockBusinessObjects;
using RoverExtras.MockModelImplementation.MockUserInterface;
using RoverTest.ModelApplicationData;
using RoverTest.ModelUserInterface;

namespace UnitTests.Sandbox;

public class TestSandbox
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void RoverBo()
    {
        RoverObjectCollections factory = new RoverObjectCollections();
        Categories cats = (Categories)factory.GetRoverCollection("Categories");
        Assert.That(cats.Count > 0);

        Items items = (Items)factory.GetRoverCollection("Items");
        Assert.That(items.Count > 0);

    }

    [Test]
    public void LoadHome()
    {
        // Mock does not need an App Driver.
        RoverPages pages = new RoverPages();
        PageHome pageHome = (PageHome)pages.GetPage("PageHome");
        RoverPageAction action = pageHome.SelectCategory(1);
        Assert.That(action.Result);
    }
}
