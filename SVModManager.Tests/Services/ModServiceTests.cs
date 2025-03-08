using SVModManager.Model;
using SVModManager.Services;

namespace SVModManager.Tests;

[TestClass]
public class ModServiceTests
{
    private static ModService _modService;
    private static DbService _dbService;
    private static FileHandler _fileHandler;
    private static JsonService _jsonService;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _dbService = new DbService();
        _fileHandler = new FileHandler();
        _jsonService = new JsonService();
        _dbService.InitializeDatabase();
        _modService = new ModService(_dbService, _fileHandler, _jsonService);
    }

    [TestMethod]
    public void TestProcessModsInDirectory()
    {
        _modService.ProcessModsInDirectory(@"F:\game\Mods");
        var mods = _modService.getAllMods();
        _modService.ExportModsToFile(@"F:\game\Mods\mods.json");
        Assert.IsTrue(mods.Count > 0, "Mods were not processed correctly.");
    }

    [TestMethod]
    public void TestAddNewMod()
    {
        var newMod = new Mod
        {
            Name = "NewMod",
            Path = @"F:\game\Mods\NewMod",
            NexusId = 12345,
            CreateOn = System.DateTime.Now,
            LastModified = System.DateTime.Now,
            Description = "This is a new mod.",
            IsEnabled = true
        };
        _modService.AddMod(newMod);
        var modInDb = _modService.GetModByName("NewMod");
        Assert.IsNotNull(modInDb, "NewMod was not added to the database.");
        Assert.AreEqual(newMod.Name, modInDb.Name, "Mod name does not match.");
    }

    [TestMethod]
    public void TestUpdateMod_ExistingMod()
    {
        var mod = _modService.GetModByName("NewMod");
        Assert.IsNotNull(mod, "TestMod was not found in the database.");
        mod.Version = "2.0";
        mod.IsEnabled = false;
        _modService.updateMod(mod);
        var updatedMod = _modService.GetModByName("NewMod");
        Assert.AreEqual("2.0", updatedMod.Version, "Mod version was not updated.");
        Assert.IsFalse(updatedMod.IsEnabled, "Mod was not disabled.");
    }

    [TestMethod]
    public void TestDisableMod_ModIsDisabled()
    {
        _modService.disableMod("More Grass");
        var mod = _modService.GetModByName("More Grass");
        Assert.IsFalse(mod.IsEnabled, "Mod was not disabled.");
    }

    [TestMethod]
    public void TestEnableMod_ModIsEnabled()
    {
        _modService.enableMod("More Grass");
        var mod = _modService.GetModByName("More Grass");
        Assert.IsTrue(mod.IsEnabled, "Mod was not enabled.");
    }

    [TestMethod]
    public void TestUpdateMods()
    {
        _modService.ProcessModsInDirectory(@"F:\game\Mods");
    }

    [TestMethod]
    public void TestClearMod()
    {
        _modService.clearAllMods();
        var mods = _modService.getAllMods();
        Assert.IsTrue(mods.Count == 0, "Mods were not cleared.");
        _modService.ProcessModsInDirectory(@"F:\game\Mods");
    }
}
