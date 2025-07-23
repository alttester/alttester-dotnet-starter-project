# AltTester Project Template

## 📁 This is a template project for automated testing using the View Object Model pattern with AltTester, Appium, and Selenium. It provides a solid foundation for building robust test automation frameworks for Unity games and applications.

## 🚀 Quick Start

1. **Copy this template** to your project directory
2. **Update locators** in `Views/MainMenuView.cs` and `Views/GamePlayView.cs` to match your game objects  
3. **Customize the tests** in `Tests/MainMenuTests.cs` and `Tests/GamePlayTests.cs` for your game's functionality
4. **Run tests** with `./run_tests.sh` (Linux/macOS) or `run_tests.bat` (Windows)

The template includes two simple views and a couple of tests that you can adapt for any game!## 🚀 Features

- **View Object Model**: Clean separation of test logic and view interactions
- **Multi-Driver Support**: AltTester, Appium (mobile), and Selenium (web) integration
- **Two Template Views**: Main menu and gameplay view objects to get you started quickly
- **Comprehensive Examples**: Template code covering common game testing scenarios
- **NUnit Framework**: Industry-standard testing framework for .NET
- **Configuration Management**: Environment-based configuration with sensible defaults
- **Utility Classes**: Common helpers for locators, reporting, and driver management
- **Easy Customization**: Template code with clear instructions for adaptation

## 📁 Project Structure

```
AltTesterProject/
├── Common/                    # Shared utilities and infrastructure
│   ├── DriverContainer.cs     # Multi-driver container
│   ├── Reporter.cs           # Test reporting utilities
│   └── TestConfiguration.cs  # Configuration management
├── Views/                     # View Object Model classes
│   ├── BaseView.cs           # Base view with common functionality
│   ├── MainMenuView.cs       # Main menu view object
│   └── GamePlayView.cs       # Game play view object
├── Tests/                     # Test classes
│   ├── BaseTest.cs           # Base test class with setup/teardown
│   └── MainMenuTests.cs      # Main menu tests
├── run_tests.sh              # Simple test runner for Linux/macOS
├── run_tests.bat             # Simple test runner for Windows
└── GlobalUsings.cs           # Global using statements
```

## 🛠️ Setup Instructions

### Prerequisites

1. **.NET 8.0 SDK** or later
2. **AltTester Unity SDK** integrated in your Unity project
3. **Visual Studio** or **Visual Studio Code** (optional but recommended)
4. **Appium Server** (if testing mobile platforms)
5. **Chrome/ChromeDriver** (if testing WebGL)

### Installation

1. Clone or copy this template project to your desired location
2. **Update the locators** in `Views/MainMenuView.cs` and `Views/GamePlayView.cs` to match your game's UI elements
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
4. Build the project:
   ```bash
   dotnet build
   ```

### Configuration

The project uses default configurations that work out of the box. You can customize settings by modifying `TestConfiguration.cs` or setting environment variables:

#### Key Environment Variables (supported by test runners):
- `ALT_TESTER_SERVER_URL` - AltTester server URL (default: "127.0.0.1")
- `ALT_TESTER_SERVER_PORT` - AltTester server port (default: 13000)
- `TEST_PLATFORM` - Target platform: Android, iOS, WebGL (default: "Android")
- `RUN_TESTS_WITH_APPIUM` - Enable Appium: true/false (default: "false")
- `RUN_TESTS_WITH_SELENIUM` - Enable Selenium: true/false (default: "false")

For additional configurations, see the `TestConfiguration.cs` file.

### Test Outputs

The test runners create a `TestReport` directory (or custom directory if specified) containing:

- `junit.xml` - JUnit test result file
- Additional test result files from dotnet test
- Screenshots and logs are saved to the build output directory

### Running Tests

1. **Start your Unity application** with AltTester integration enabled
2. **Run tests** using the simple test runners:

   **Linux/macOS:**
   ```bash
   # Run all tests
   ./run_tests.sh
   
   # Run specific test class
   ./run_tests.sh MainMenuTests
   
   # Run specific test class with custom output directory
   ./run_tests.sh MainMenuTests results
   
   # Run with environment variables
   ALT_TESTER_SERVER_PORT=13001 ./run_tests.sh
   TEST_PLATFORM=WebGL RUN_TESTS_WITH_SELENIUM=true ./run_tests.sh
   ```

   **Windows:**
   ```cmd
   REM Run all tests
   run_tests.bat
   
   REM Run specific test class
   run_tests.bat MainMenuTests
   
   REM Run specific test class with custom output directory
   run_tests.bat MainMenuTests results
   
   REM Run with environment variables
   set ALT_TESTER_SERVER_PORT=13001 && run_tests.bat
   set TEST_PLATFORM=WebGL && set RUN_TESTS_WITH_SELENIUM=true && run_tests.bat
   ```

   **Or use dotnet CLI directly:**
   ```bash
   # Run all tests with detailed output
   dotnet test --logger "console;verbosity=detailed"
   
   # Run specific test class
   dotnet test --filter "MainMenuTests"
   ```

## 📝 Usage Examples

### Creating a New View Object

**Example 1: Custom Main Menu View**
```csharp
namespace AltTesterProject.Views;

public class MyCustomMenuView : BaseView
{
    // Define locators for your specific view
    private readonly (By, string) PlayButton = (By.NAME, "PlayButtonName");
    private readonly (By, string) SettingsButton = (By.NAME, "SettingsButtonName");

    public MyCustomMenuView(DriverContainer drivers) : base(drivers)
    {
    }

    // Define view actions
    public void ClickPlay()
    {
        var playButton = FindElement(PlayButton);
        playButton.Click();
    }

    public void OpenSettings()
    {
        var settingsButton = FindElement(SettingsButton);
        settingsButton.Click();
    }
}
```

**Example 2: Custom Gameplay View**
```csharp
namespace AltTesterProject.Views;

public class MyCustomGamePlayView : BaseView
{
    // Define locators for your specific gameplay elements
    private readonly (By, string) Player = (By.NAME, "PlayerCharacter");
    private readonly (By, string) HealthBar = (By.NAME, "HealthBar");

    public MyCustomGamePlayView(DriverContainer drivers) : base(drivers)
    {
    }

    // Define gameplay actions
    public void MovePlayer(float x, float y)
    {
        var player = FindElement(Player);
        // Custom movement logic here
    }

    public int GetPlayerHealth()
    {
        var healthBar = FindElement(HealthBar);
        // Extract health value logic here
        return 100;
    }
}
```

### Creating a New Test Class

```csharp
namespace AltTesterProject.Tests;

[TestFixture]
[AllureFeature("My Custom Feature")]
public class MyCustomTests : BaseTest
{
    private MyCustomMenuView? _menuView;
    private MyCustomGamePlayView? _gamePlayView;

    [SetUp]
    public void TestSetUp()
    {
        _menuView = new MyCustomMenuView(Drivers);
        _gamePlayView = new MyCustomGamePlayView(Drivers);
    }

    [Test]
    [AllureTest("Test game flow from menu to gameplay")]
    public void TestGameFlow()
    {
        Reporter.Log("Starting from main menu");
        _menuView!.ClickPlay();

        Reporter.Log("Verifying gameplay loaded");
        Assert.That(_gamePlayView!.GetPlayerHealth(), Is.GreaterThan(0));
    }
}
```

## 🔧 Customization

### Updating Locators
1. Open `Views/MainMenuView.cs` and `Views/GamePlayView.cs`
2. Replace the example locators with actual game object names, IDs, or paths from your Unity project
3. Update the locator strategies (ByName, ByTag, ByComponent) to match your game's structure

### Adding New View Objects
Create new view classes that inherit from `BaseView` following the pattern in `MainMenuView.cs` and `GamePlayView.cs`

### Extending Tests
Add new test methods to the existing test classes or create new test classes following the same pattern

## 📊 Reporting

The project includes basic test reporting:

1. **JUnit XML reports** are generated automatically in the output directory
2. **Console output** shows detailed test execution with verbose logging
3. **Screenshots and logs** are captured automatically on test failures

For advanced reporting, you can integrate Allure or other reporting tools as needed.

## 🐛 Troubleshooting

### Common Issues

1. **Connection refused**: Ensure your Unity application is running with AltTester enabled
2. **Object not found**: Verify locators match actual game object names/paths
3. **Timeout errors**: Increase timeout values or check if objects are actually present
4. **Driver startup failures**: Check that required services (Appium, etc.) are running

### Debug Tips

- Use `Reporter.Log()` for detailed logging during test execution
- Screenshots are automatically taken on test failures and saved to the build output directory
- Unity logs are captured automatically and saved alongside screenshots
- Check Unity Console for AltTester connection logs
- The test runners show detailed output to help with debugging
- Test results are saved in JUnit XML format for analysis

## 🤝 Getting Started

1. **Update locators**: Edit `Views/MainMenuView.cs` and `Views/GamePlayView.cs` to match your game's UI elements
2. **Customize the tests**: Modify `Tests/MainMenuTests.cs` to test your specific game functionality
3. **Add more views**: Create additional view objects for other screens (settings, inventory, etc.)
4. **Extend tests**: Add more test methods or create new test classes as needed

This template provides a solid foundation with two common game views that you can build upon for any Unity game testing project.

## 📚 Additional Resources

- [AltTester Documentation](https://altom.com/alttester/)
- [NUnit Documentation](https://docs.nunit.org/)
- [Allure Documentation](https://docs.qameta.io/allure/)
- [Appium Documentation](https://appium.io/docs/)
- [Selenium Documentation](https://selenium-python.readthedocs.io/)

## 📄 License

This template is provided as-is for educational and development purposes. Modify and adapt as needed for your projects.
