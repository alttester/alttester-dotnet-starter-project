
namespace AltTesterProject.Tests
{
    [TestFixture]
    [AllureSuite("Main Menu Tests")]
    public class MainMenuTests : BaseTest
    {
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

            MainMenuView.WaitForMainMenuReady(timeoutSeconds: 2);
            MainMenuView.StartNewGame(playerName: "TestPlayer");

            GamePlayView.WaitForGamePlayReady(timeoutSeconds: 2);
            Assert.That(GamePlayView.IsMainCharacterPresent(), Is.True, "Main character should be present after starting a new game");

        }
    }
}