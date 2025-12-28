@echo off
chcp 65001 >nul
echo Building School solution...
dotnet build School.sln
if %errorlevel% equ 0 (
    echo.
    echo Build successful!
    echo.
    echo To run the application, execute: run.cmd
) else (
    echo.
    echo Build failed! Please check the errors above.
)
pause

