@echo off
chcp 65001 >nul
echo ╔═══════════════════════════════════════════════════════════════╗
echo ║  Запуск Lab2 з захопленням виводу для PDF                    ║
echo ╚═══════════════════════════════════════════════════════════════╝
echo.

echo [1/4] Збірка рішення...
dotnet build School.sln
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Помилка збірки!
    pause
    exit /b 1
)
echo ✅ Збірка успішна!
echo.

echo [2/4] Запуск консольного застосунку...
dotnet run --project School.Console\School.Console.csproj > lab2_output.txt 2>&1
echo ✅ Програму виконано!
echo.

echo [3/4] Запуск тестів...
dotnet test School.Tests\School.Tests.csproj > lab2_tests.txt 2>&1
echo ✅ Тести виконано!
echo.

echo [4/4] Результати збережено у файли:
echo    📄 lab2_output.txt - вивід програми
echo    📄 lab2_tests.txt - результати тестів
echo    📄 teachers_data.json - згенеровані дані
echo.

echo ╔═══════════════════════════════════════════════════════════════╗
echo ║  Тепер створіть PDF з файлів lab2_output.txt та             ║
echo ║  lab2_tests.txt використовуючи Word або онлайн сервіс        ║
echo ╚═══════════════════════════════════════════════════════════════╝
echo.

pause

