using By = AltTester.AltTesterUnitySDK.Driver.By;

namespace AltTesterProject.Views
{
    public class GamePlayView : BaseView
    {
        private readonly (By, string) PauseButtonLocator = (By.NAME, "PauseButton");
        private readonly (By, string) ResumeButtonLocator = (By.NAME, "ResumeButton");
        private readonly (By, string) MainCharacterLocator = (By.NAME, "MainCharacter");
        private readonly (By, string) GameHudLocator = (By.NAME, "GameHUD");

        public GamePlayView(DriverContainer driverContainer) : base(driverContainer)
        {
        }

        [AllureStep("Pause the game")]
        public void PauseGame()
        {
            var pauseButton = FindObject(PauseButtonLocator);
            pauseButton.Click();
            Reporter.Log("Game paused");
        }

        [AllureStep("Resume the game")]
        public void ResumeGame()
        {
            var resumeButton = FindObject(ResumeButtonLocator);
            resumeButton.Click();
            Reporter.Log("Game resumed");
        }

        [AllureStep("Check if game is paused")]
        public bool IsGamePaused()
        {
            try
            {
                var resumeButton = FindObject(ResumeButtonLocator);
                var isVisible = resumeButton.enabled;
                Reporter.Log($"Game paused: {isVisible}");
                return isVisible;
            }
            catch
            {
                Reporter.Log("Resume button not found - game is not paused");
                return false;
            }
        }

        [AllureStep("Check if main character is present")]
        public bool IsMainCharacterPresent()
        {
            try
            {
                var mainCharacter = FindObject(MainCharacterLocator);
                var isPresent = mainCharacter.enabled;
                Reporter.Log($"Main character present: {isPresent}");
                return isPresent;
            }
            catch
            {
                Reporter.Log("Main character not found");
                return false;
            }
        }

        [AllureStep("Get main character position")]
        public (float x, float y, float z) GetMainCharacterPosition()
        {
            var mainCharacter = FindObject(MainCharacterLocator);
            var position = mainCharacter.GetWorldPosition();
            Reporter.Log($"Main character position: {position.x}, {position.y}, {position.z}");
            return (position.x, position.y, position.z);
        }

        [AllureStep("Wait for gameplay to be ready")]
        public void WaitForGamePlayReady(int timeoutSeconds = 10)
        {
            WaitForObject(GameHudLocator, timeoutSeconds);
            Reporter.Log("Gameplay is ready");
        }

        [AllureStep("Check if gameplay HUD is visible")]
        public bool IsGamePlayHudVisible()
        {
            try
            {
                var gameHud = FindObject(GameHudLocator);
                var isVisible = gameHud.enabled;
                Reporter.Log($"Gameplay HUD visible: {isVisible}");
                return isVisible;
            }
            catch
            {
                Reporter.Log("Gameplay HUD not found");
                return false;
            }
        }

        // TODO: Add your own gameplay-specific methods here
        // Examples:
        // - MoveCharacter(Direction direction)
        // - UseAbility(string abilityName)
        // - CollectItem(string itemName)
        // - CheckHealth()
        // - GetScore()
        // etc.
    }
}
