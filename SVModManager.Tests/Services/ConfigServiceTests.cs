namespace SVModManager.Tests;
using SVModManager.Services;
using SVModManager.Model;

[TestClass]
public class ConfigServiceTests
{
    public static ConfigService _configService;
    public static DbService _dbService;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        _dbService = new DbService();
        _dbService.InitializeDatabase();
        _configService = new ConfigService(_dbService);
        _configService.InitConfig();
    }

    [TestMethod]
    public void TestAutoPath()
    {
        _configService.autoGetModPath();
        Assert.IsNotNull(_configService.getStardewModPath(), "Mod path was not set.");
    }

    [TestMethod]
    public void TestSetStardewModPath()
    {
        _configService.setStardewModPath("C:\\TestPath");
        Assert.AreEqual("C:\\TestPath", _configService.getStardewModPath(), "Mod path does not match.");
    }

    [TestMethod]
    public void TestSetNexusAPI()
    {
        _configService.setNexusAPI("TestAPI");
        Assert.AreEqual("TestAPI", _configService.getNexusAPI(), "API does not match.");
    }

    [TestMethod]
    public void TestGetNexusAPI()
    {
        _configService.setNexusAPI("TestAPI");
        Assert.AreEqual("TestAPI", _configService.getNexusAPI(), "API does not match.");
    }

    [TestMethod]
    public void TestGetStardewModPath()
    {
        _configService.setStardewModPath("C:\\TestPath");
        Assert.AreEqual("C:\\TestPath", _configService.getStardewModPath(), "Mod path does not match.");
    }

}
