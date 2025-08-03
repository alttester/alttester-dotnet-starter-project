using AltBy = AltTester.AltTesterUnitySDK.Driver.By;
using SeleniumBy = OpenQA.Selenium.By;

namespace AltTesterProject.Tests
{
    [TestFixture]
    [AllureSuite("Main Menu Tests")]
    public class MainMenuTests : BaseTest
    {
        private MainMenuView mainMenuView;
        private GamePlayView gamePlayView;

        [SetUp]
        public void TestSetUp()
        {
            mainMenuView = new MainMenuView(Drivers);
            gamePlayView = new GamePlayView(Drivers);
        }

        [Test]
        public void TestMainMenuLoadsSuccessfully()
        {
            // This test should always pass, since there should always be a scene loaded

            Reporter.Log("Testing main menu loads successfully", withScreenshot: true);
            var currentScene = Drivers.AltDriver.GetCurrentScene();
            Assert.That(currentScene, Is.Not.Empty, "Game did not launch successfully, expected to have a scene loaded.");

        }

        [Test]
        public void TestCanStartNewGame()
        {
            // This test will should fail because it's expecting an element you probably don't have in your scene

            mainMenuView.WaitForMainMenuReady(timeoutSeconds: 2);
            mainMenuView.StartNewGame(playerName: "TestPlayer");

            gamePlayView.WaitForGamePlayReady(timeoutSeconds: 2);
            Assert.That(gamePlayView.IsMainCharacterPresent(), Is.True, "Main character should be present after starting a new game");

        }
    }
}