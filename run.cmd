@echo off
chcp 65001 >nul

echo ╔═══════════════════════════════════════════════════════════════╗
echo ║  Лабораторна робота №2 - Запуск програми                     ║
echo ╚═══════════════════════════════════════════════════════════════╝
echo.

echo [1/2] Збірка рішення...
dotnet build School.sln
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Помилка збірки!
    pause
    exit /b 1
)
echo ✅ Збірка успішна!
echo.

echo [2/2] Запуск програми...
echo.
dotnet run --project School.Console\School.Console.csproj
