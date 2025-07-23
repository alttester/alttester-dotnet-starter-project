using By = AltTester.AltTesterUnitySDK.Driver.By;

namespace AltTesterProject.Views
{
    public abstract class BaseView
    {
        protected DriverContainer Drivers { get; set; }

        protected AltDriver AltDriver => Drivers.AltDriver;
        protected AppiumDriver<AppiumWebElement>? AppiumDriver => Drivers.AppiumDriver;
        protected IWebDriver? SeleniumDriver => Drivers.SeleniumDriver;

        protected BaseView(DriverContainer drivers)
        {
            Drivers = drivers;
        }

        [AllureStep("Click on object")]
        public virtual void ClickObject((By, string) locator, float timeout = 10.0f, bool waitForClick = true)
        {
            var altObject = WaitForObject(locator, timeout);
            altObject.Click(wait: waitForClick);
            Thread.Sleep(500); // Brief pause after click
         }

        [AllureStep("Tap on object")]
        public virtual void TapObject((By, string) locator, int count = 1, float timeout = 10.0f)
        {
            var altObject = WaitForObject(locator, timeout);
            altObject.Tap(count);
            Thread.Sleep(500); // Brief pause after tap
        }

        [AllureStep("Wait for object")]
        public virtual AltObject WaitForObject((By, string) locator, float timeout = 20.0f, double interval = 0.5)
        {
            Reporter.Log($"Waiting for element {locator.Item2} to be present.");
            try
            {
                return AltDriver.WaitForObject(locator.Item1, locator.Item2, timeout: timeout, interval: interval);
            }
            catch (AltTester.AltTesterUnitySDK.Driver.WaitTimeOutException)
            {
                Reporter.Log($"Element {locator.Item2} was not found within {timeout} seconds", withScreenshot: true);
                throw new AssertionException($"Element '{locator.Item2}' was not found within {timeout} seconds. Please check if the element exists or if the game loaded correctly.");
            }
        }

        [AllureStep("Wait for object which contains")]
        public virtual AltObject WaitForObjectWhichContains((By, string) locator, float timeout = 20.0f)
        {
            return AltDriver.WaitForObjectWhichContains(locator.Item1, locator.Item2, timeout: timeout);
        }

        [AllureStep("Wait for object not to be present")]
        public virtual void WaitForObjectNotBePresent((By, string) locator, float timeout = 20.0f)
        {
            AltDriver.WaitForObjectNotBePresent(locator.Item1, locator.Item2, timeout: timeout);
        }

        [AllureStep("Set text on object")]
        public virtual void SetText((By, string) locator, string text, float timeout = 10.0f)
        {
            var altObject = WaitForObject(locator, timeout);
            altObject.SetText(text);
        }

        [AllureStep("Get text from object")]
        public virtual string GetText((By, string) locator, float timeout = 10.0f)
        {
            var altObject = WaitForObject(locator, timeout);
            return altObject.GetText();
        }

        [AllureStep("Check if object is present")]
        public virtual bool IsObjectPresent((By, string) locator)
        {
            try
            {
                AltDriver.FindObject(locator.Item1, locator.Item2);
                return true;
            }
            catch (AltTester.AltTesterUnitySDK.Driver.NotFoundException)
            {
                return false;
            }
        }
        [AllureStep("Find element by locator")]
        public virtual AltObject FindElement((By, string) locator)
        {
            try
            {
                return AltDriver.FindObject(locator.Item1, locator.Item2);
            }
            catch (AltTester.AltTesterUnitySDK.Driver.NotFoundException)
            {
                Reporter.Log($"Element {locator.Item2} not found", withScreenshot: true);
                throw new AssertionException($"Element '{locator.Item2}' was not found. Please verify the element exists in the current scene.");
            }
        }

        [AllureStep("Get current scene")]
        public virtual string GetCurrentScene()
        {
            return AltDriver.GetCurrentScene();
        }

        [AllureStep("Load scene")]
        public virtual void LoadScene(string sceneName)
        {
            AltDriver.LoadScene(sceneName);
        }

        [AllureStep("Take screenshot")]
        public virtual void TakeScreenshot(string path)
        {
            AltDriver.GetPNGScreenshot(path);
        }

        [AllureStep("Wait for a specified duration")]
        public virtual void Wait(double seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
        }
    }
}
