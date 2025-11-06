
namespace AltTesterProject.Views
{
    public class MainMenuView : BaseView
    {
        private readonly (By, string) PlayButtonLocator = (By.NAME, "PlayButton");
        private readonly (By, string) MainMenuPanelLocator = (By.NAME, "MainMenuPanel");
        private readonly (By, string) PlayerNameInputLocator = (By.NAME, "PlayerNameInput");
        private readonly (By, string) SettingsButtonLocator = (By.NAME, "SettingsButton");

        public MainMenuView(DriverContainer driverContainer) : base(driverContainer)
        {
        }

        [AllureStep("Click play button")]
        public void ClickPlayButton()
        {
            var playButton = FindObject(PlayButtonLocator);
            playButton.Click();
            Reporter.Log("Clicked play button");
        }

        [AllureStep("Check if main menu is visible")]
        public bool IsMainMenuVisible()
        {
            try
            {
                var mainMenuPanel = FindObject(MainMenuPanelLocator);
                var isVisible = mainMenuPanel.enabled;
                Reporter.Log($"Main menu panel visible: {isVisible}");
                return isVisible;
            }
            catch
            {
                Reporter.Log("Main menu panel not found");
                return false;
            }
        }

        [AllureStep("Enter player name")]
        public void EnterPlayerName(string playerName)
        {
            var inputField = FindObject(PlayerNameInputLocator);
            inputField.SetText(playerName, true);
            Reporter.Log($"Entered player name: {playerName}");
        }

        [AllureStep("Navigate to settings")]
        public void NavigateToSettings()
        {
            var settingsButton = FindObject(SettingsButtonLocator);
            settingsButton.Click();
            Reporter.Log("Navigated to settings");
        }

        [AllureStep("Wait for main menu to be ready")]
        public void WaitForMainMenuReady(int timeoutSeconds = 10)
        {
            WaitForObject(MainMenuPanelLocator, timeoutSeconds);
            Reporter.Log("Main menu is ready");
        }

        [AllureStep("Start new game")]
        public void StartNewGame(string playerName)
        {
            WaitForMainMenuReady();

            if (!IsMainMenuVisible())
            {
                throw new Exception("Main menu is not visible, cannot start new game");
            }

            EnterPlayerName(playerName);
            ClickPlayButton();

            Reporter.Log($"Started new game for player: {playerName}");
        }

        // TODO: Add your own main menu-specific methods here
        // Examples:
        // - NavigateToLevelSelect()
        // - ShowLeaderboard()
        // - NavigateToStore()
        // - ShowAchievements()
        // - SelectGameMode(string mode)
        // etc.
    }
}