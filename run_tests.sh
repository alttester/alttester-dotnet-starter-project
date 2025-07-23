#!/bin/bash

# Simple AltTester test runner

# Defaults
FILTER=""
OUTPUT_DIR="TestReport"

# Usage
if [[ "$1" == "--help" || "$1" == "-h" ]]; then
    echo "Usage: $0 [test_filter] [output_dir]"
    echo "Examples:"
    echo "  $0                           # Run all tests"
    echo "  $0 MainMenuTests             # Run only MainMenuTests"
    echo "  $0 GamePlayTests results     # Run GamePlayTests, save to 'results' dir"
    echo ""
    echo "Environment Variables:"
    echo "  ALT_TESTER_SERVER_URL        # AltTester server URL (default: 127.0.0.1)"
    echo "  ALT_TESTER_SERVER_PORT       # AltTester server port (default: 13000)"
    echo "  TEST_PLATFORM                # Target platform: Android, iOS, WebGL (default: Android)"
    echo "  RUN_TESTS_WITH_APPIUM        # Enable Appium: true/false (default: false)"
    echo "  RUN_TESTS_WITH_SELENIUM      # Enable Selenium: true/false (default: false)"
    echo ""
    echo "Examples with environment variables:"
    echo "  ALT_TESTER_SERVER_PORT=13001 $0"
    echo "  TEST_PLATFORM=WebGL RUN_TESTS_WITH_SELENIUM=true $0"
    exit 0
fi

# Parse simple arguments
[[ -n "$1" ]] && FILTER="$1"
[[ -n "$2" ]] && OUTPUT_DIR="$2"

echo "Running AltTester tests..."
[[ -n "$FILTER" ]] && echo "Filter: $FILTER"
echo "Output: $OUTPUT_DIR"

# Show environment variables if set
[[ -n "$ALT_TESTER_SERVER_URL" ]] && echo "AltTester URL: $ALT_TESTER_SERVER_URL"
[[ -n "$ALT_TESTER_SERVER_PORT" ]] && echo "AltTester Port: $ALT_TESTER_SERVER_PORT"
[[ -n "$TEST_PLATFORM" ]] && echo "Platform: $TEST_PLATFORM"
[[ -n "$RUN_TESTS_WITH_APPIUM" ]] && echo "Appium: $RUN_TESTS_WITH_APPIUM"
[[ -n "$RUN_TESTS_WITH_SELENIUM" ]] && echo "Selenium: $RUN_TESTS_WITH_SELENIUM"
echo

# Create output directory
mkdir -p "$OUTPUT_DIR"

# Build and run tests
dotnet build --configuration Debug || { echo "Build failed"; exit 1; }

# Run tests
if [[ -n "$FILTER" ]]; then
    dotnet test --filter "$FILTER" --logger "console;verbosity=detailed" --logger "junit;LogFileName=junit.xml" --results-directory "$OUTPUT_DIR"
    TEST_EXIT_CODE=$?
else
    dotnet test --logger "console;verbosity=detailed" --logger "junit;LogFileName=junit.xml" --results-directory "$OUTPUT_DIR"
    TEST_EXIT_CODE=$?
fi

if [[ $TEST_EXIT_CODE -eq 0 ]]; then
    echo "✅ Tests passed! Results in $OUTPUT_DIR"
else
    echo "❌ Some tests failed (exit code: $TEST_EXIT_CODE). Results in $OUTPUT_DIR"
fi

# Generate Allure report if allure-results exist
if [[ -d "bin/Debug/net8.0/allure-results" ]]; then
    echo "Generating Allure report..."
    if command -v allure &> /dev/null; then
        allure generate bin/Debug/net8.0/allure-results -o "$OUTPUT_DIR/allure-report" --clean --single-file
        echo "📊 Allure report generated in $OUTPUT_DIR/allure-report"
    else
        echo "⚠️  Allure command not found. Install with: npm install -g allure-commandline"
        echo "   Allure results available in: bin/Debug/net8.0/allure-results"
    fi
fi

# Exit with the original test exit code after generating reports
exit $TEST_EXIT_CODE
