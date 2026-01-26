using NUnit.Framework;
using RoverExtras.MockModelImplementation.MockBusinessObjects;
using RoverTest.ModelApplicationData;

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
}
