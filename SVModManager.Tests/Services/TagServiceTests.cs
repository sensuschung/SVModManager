namespace SVModManager.Tests;
using SVModManager.Model;
using SVModManager.Services;

[TestClass]
public class TagServiceTests
{
    public static TagService _tagService;
    public static DbService _dbService;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _dbService = new DbService();
        _dbService.InitializeDatabase();
        _tagService = new TagService(_dbService);
    }

    [TestMethod]
    public void TestAddTag()
    {
        _tagService.AddTag("TestTag");
        var tag = _dbService.QueryItem<Tag>(m => m.Name == "TestTag");
        Assert.IsNotNull(tag, "TestTag was not added to the database.");
        Assert.AreEqual("TestTag", tag.Name, "Tag name does not match.");
    }

    [TestMethod]
    public void TestRemoveTag()
    {
        _tagService.AddTag("TestRemoveTag");
        var tag = _dbService.QueryItem<Tag>(m => m.Name == "TestRemoveTag");
        Assert.IsNotNull(tag, "TestRemoveTag was not added to the database.");
        _tagService.RemoveTag("TestRemoveTag");
        tag = _dbService.QueryItem<Tag>(m => m.Name == "TestRemoveTag");
        Assert.IsNull(tag, "TestRemoveTag was not removed from the database.");
    }

    [TestMethod]
    public void TestAddTagtoMod()
    {
        _tagService.AddTag("TestTagtoMod");
        var tag = _dbService.QueryItem<Tag>(m => m.Name == "TestTagtoMod");
        var mod = _dbService.QueryItem<Mod>(m => m.Name == "More Grass");
        Assert.IsNotNull(tag, "TestTagtoMod was not added to the database.");
        Assert.IsNotNull(mod, "MoreGrass was not found in the database.");
        mod.Tags.Add(tag);
        _dbService.UpdateItem(mod);
    }

    [TestMethod]
    public void TestRemoveTagFromMod()
    {
        var tag = _dbService.QueryItem<Tag>(m => m.Name == "TestTagtoMod");
        var mod = _dbService.QueryItem<Mod>(m => m.Name == "More Grass");
        Assert.IsNotNull(tag, "TestTagtoMod was not found in the database.");
        Assert.IsNotNull(mod, "MoreGrass was not found in the database.");
        mod.Tags.Remove(tag);
        _dbService.UpdateItem(mod);
    }

    [TestMethod]
    public void TestRmoveNotEmptyTag()
    {
        var tag = _dbService.QueryItem<Tag>(m => m.Name == "TestTagtoMod");
        var mod = _dbService.QueryItem<Mod>(m => m.Name == "More Grass");
        Assert.IsNotNull(tag, "TestTagtoMod was not added to the database.");
        Assert.IsNotNull(mod, "MoreGrass was not found in the database.");
        _tagService.RemoveTag("TestTagtoMod");
    }
}
