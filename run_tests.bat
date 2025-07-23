@echo off
setlocal

REM Simple AltTester test runner for Windows

REM Defaults
set "FILTER="
set "OUTPUT_DIR=TestReport"

REM Usage
if "%1"=="--help" goto :usage
if "%1"=="-h" goto :usage
if "%1"=="/?" goto :usage
goto :parse_args

:usage
echo Usage: %0 [test_filter] [output_dir]
echo Examples:
echo   %0                           # Run all tests
echo   %0 MainMenuTests             # Run only MainMenuTests
echo   %0 GamePlayTests results     # Run GamePlayTests, save to 'results' dir
echo.
echo Environment Variables:
echo   ALT_TESTER_SERVER_URL        # AltTester server URL (default: 127.0.0.1)
echo   ALT_TESTER_SERVER_PORT       # AltTester server port (default: 13000)
echo   TEST_PLATFORM                # Target platform: Android, iOS, WebGL (default: Android)
echo   RUN_TESTS_WITH_APPIUM        # Enable Appium: true/false (default: false)
echo   RUN_TESTS_WITH_SELENIUM      # Enable Selenium: true/false (default: false)
echo.
echo Examples with environment variables:
echo   set ALT_TESTER_SERVER_PORT=13001 ^&^& %0
echo   set TEST_PLATFORM=WebGL ^&^& set RUN_TESTS_WITH_SELENIUM=true ^&^& %0
exit /b 0

:parse_args
REM Parse simple arguments
if not "%1"=="" set "FILTER=%1"
if not "%2"=="" set "OUTPUT_DIR=%2"

echo Running AltTester tests...
if not "%FILTER%"=="" echo Filter: %FILTER%
echo Output: %OUTPUT_DIR%

REM Show environment variables if set
if not "%ALT_TESTER_SERVER_URL%"=="" echo AltTester URL: %ALT_TESTER_SERVER_URL%
if not "%ALT_TESTER_SERVER_PORT%"=="" echo AltTester Port: %ALT_TESTER_SERVER_PORT%
if not "%TEST_PLATFORM%"=="" echo Platform: %TEST_PLATFORM%
if not "%RUN_TESTS_WITH_APPIUM%"=="" echo Appium: %RUN_TESTS_WITH_APPIUM%
if not "%RUN_TESTS_WITH_SELENIUM%"=="" echo Selenium: %RUN_TESTS_WITH_SELENIUM%
echo.

REM Create output directory
if not exist "%OUTPUT_DIR%" mkdir "%OUTPUT_DIR%"

REM Build and run tests
echo Building project...
dotnet build --configuration Debug
if errorlevel 1 (
    echo Build failed
    exit /b 1
)

REM Run tests
echo Running tests...
if not "%FILTER%"=="" (
    dotnet test --filter "%FILTER%" --logger "console;verbosity=detailed" --logger "junit;LogFileName=junit.xml" --results-directory "%OUTPUT_DIR%"
    set TEST_EXIT_CODE=%errorlevel%
) else (
    dotnet test --logger "console;verbosity=detailed" --logger "junit;LogFileName=junit.xml" --results-directory "%OUTPUT_DIR%"
    set TEST_EXIT_CODE=%errorlevel%
)

if %TEST_EXIT_CODE% equ 0 (
    echo ✅ Tests passed! Results in %OUTPUT_DIR%
) else (
    echo ❌ Some tests failed (exit code: %TEST_EXIT_CODE%). Results in %OUTPUT_DIR%
)

REM Generate Allure report if allure-results exist
if exist "bin\Debug\net8.0\allure-results" (
    echo Generating Allure report...
    where allure >nul 2>nul
    if %errorlevel% equ 0 (
        allure generate bin\Debug\net8.0\allure-results -o "%OUTPUT_DIR%\allure-report" --clean --single-file
        echo 📊 Allure report generated in %OUTPUT_DIR%\allure-report
    ) else (
        echo ⚠️  Allure command not found. Install with: npm install -g allure-commandline
        echo    Allure results available in: bin\Debug\net8.0\allure-results
    )
)

REM Exit with the original test exit code after generating reports
exit /b %TEST_EXIT_CODE%
