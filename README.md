# School - Лабораторні роботи 1 і 2

## Опис проєкту

Цей проєкт демонструє об'єктно-орієнтоване програмування на C# з використанням моделі школи, асинхронного програмування, багатопотоковості та примітивів синхронізації.

## Структура рішення

### School.Common
Бібліотека класів, що містить:
- **Базові класи та наслідування:**
  - `Person` - базовий клас для персон
  - `Student : Person` - клас студента
  - `Teacher : Person` - клас викладача
  - `Course` - клас курсу
  - `Grade` - клас оцінки

### School.Console
Консольний застосунок, що демонструє роботу CRUD сервісу та асинхронного CRUD сервісу з багатопотоковістю.

### School.Tests
Проєкт з модульними тестами для `CrudServiceAsync<T>` з використанням xUnit.

## Реалізовані конструкції

✅ **Конструктори** - у всіх класах (за замовчуванням та з параметрами)
✅ **Методи** - бізнес-логіка (CalculateAge, GetFullName, Enroll, тощо)
✅ **Статичні поля** - _totalPersonsCreated, MinimumSalary
✅ **Статичні конструктори** - ініціалізація статичних даних
✅ **Статичні методи** - GetTotalPersonsCreated, CalculateAnnualSalary
✅ **Делегати** - PersonEventHandler, GradeEventHandler
✅ **Події** - StudentEnrolled, GradeAssigned
✅ **Методи розширення** - GetInitials, GetStudentSummary, ToTitleCase

## CRUD Сервіси

### Лабораторна 1: Синхронний CRUD Сервіс
Реалізовано generic CRUD сервіс `CrudService<T>` з інтерфейсом `ICrudService<T>`:
- `Create(T element)` - створення нового елемента
- `Read(Guid id)` - читання елемента за ID
- `ReadAll()` - читання всіх елементів
- `Update(T element)` - оновлення елемента
- `Remove(T element)` - видалення елемента

### Лабораторна 2: Асинхронний CRUD Сервіс
Реалізовано багатопотоково-безпечний async CRUD сервіс `CrudServiceAsync<T>` з інтерфейсом `ICrudServiceAsync<T>`:
- `Task<bool> CreateAsync(T element)` - асинхронне створення елемента
- `Task<T> ReadAsync(Guid id)` - асинхронне читання за ID
- `Task<IEnumerable<T>> ReadAllAsync()` - читання всіх елементів
- `Task<IEnumerable<T>> ReadAllAsync(int page, int amount)` - читання з пагінацією
- `Task<bool> UpdateAsync(T element)` - асинхронне оновлення
- `Task<bool> RemoveAsync(T element)` - асинхронне видалення
- `Task<bool> SaveAsync()` - збереження колекції у JSON файл

#### Особливості реалізації:
- **Thread-safe**: Використання `ConcurrentDictionary<Guid, T>` для безпечного багатопотокового доступу
- **Async/Await**: Всі операції асинхронні
- **Серіалізація**: Збереження та завантаження даних у JSON форматі
- **Пагінація**: Вбудована підтримка постраничного читання
- **IEnumerable<T>**: Підтримка LINQ операцій

## Як побудувати та запустити

### Варіант 1: Через Visual Studio
1. Відкрийте `School.sln` у Visual Studio
2. Натисніть F5 або Ctrl+F5 для запуску

### Варіант 2: Через командний рядок
```cmd
cd "C:\Users\kawau\OneDrive\Рабочий стол\dotnet\lab1"
dotnet build School.sln
dotnet run --project School.Console\School.Console.csproj
```

### Варіант 3: Через PowerShell (якщо є проблеми з кодуванням)
```powershell
cd "C:\Users\kawau\OneDrive\Рабочий стол\dotnet\lab1"
& dotnet build School.sln
& dotnet run --project School.Console\School.Console.csproj
```

## Що демонструє програма

### Лабораторна 1:
1. Створення CRUD сервісів для Student, Teacher, Course, Grade
2. Створення об'єктів (викладачі, студенти, курси)
3. Виведення всіх об'єктів
4. Читання конкретного об'єкта за ID
5. Оновлення об'єктів (збільшення зарплати, оновлення GPA)
6. Створення та призначення оцінок
7. Видалення об'єктів
8. Демонстрація статичних методів
9. Демонстрація методів розширення
10. Демонстрація подій (StudentEnrolled, GradeAssigned)

### Лабораторна 2:
1. **Паралельне створення 1500+ об'єктів Teacher** з використанням `Parallel.For`
2. **Lock** - безпечне оновлення лічильників у багатопотоковому середовищі
3. **Semaphore** - обмеження кількості одночасних операцій (максимум 3)
4. **AutoResetEvent** - сигналізація про завершення обробки пакетів даних
5. **Monitor.Wait/Pulse** - синхронізація Producer-Consumer
6. **Статистичний аналіз**:
   - Мінімальна/Максимальна/Середня зарплата
   - Мінімальний/Максимальний/Середній вік
   - Розподіл викладачів по департаментах
7. **Пагінація** - читання даних сторінками
8. **Збереження у JSON** - серіалізація всієї колекції у файл
9. **Unit тести** - комплексне тестування з xUnit

## Структура файлів

```
School/
├── School.sln
├── School.Common/
│   ├── Person.cs (базовий клас)
│   ├── Student.cs (наслідується від Person)
│   ├── Teacher.cs (наслідується від Person + CreateNew())
│   ├── Course.cs
│   ├── Grade.cs
│   ├── PersonExtensions.cs (методи розширення)
│   ├── ICrudService.cs (синхронний інтерфейс)
│   ├── CrudService.cs (синхронна реалізація)
│   ├── ICrudServiceAsync.cs (асинхронний інтерфейс) 🆕
│   ├── CrudServiceAsync.cs (async thread-safe реалізація) 🆕
│   └── School.Common.csproj
├── School.Console/
│   ├── Program.cs (демонстрація асинхронного сервісу)
│   └── School.Console.csproj
└── School.Tests/ 🆕
    ├── CrudServiceAsyncTests.cs (unit тести)
    └── School.Tests.csproj
```

## Використані технології

- .NET 8.0
- C# 12
- Generic Types
- LINQ
- Events and Delegates
- Extension Methods
- **Async/Await** 🆕
- **Task Parallel Library (TPL)** 🆕
- **ConcurrentDictionary** 🆕
- **Synchronization Primitives** (Lock, Semaphore, AutoResetEvent, Monitor) 🆕
- **System.Text.Json** 🆕
- **xUnit Testing Framework** 🆕

## Як запустити тести

```cmd
dotnet test School.Tests\School.Tests.csproj
```

## Результати виконання

Після запуску програми:
1. Створюється файл `teachers_data.json` з серіалізованими даними
2. В консолі відображається:
   - Прогрес створення об'єктів
   - Демонстрація примітивів синхронізації
   - Статистичний аналіз даних
   - Результати збереження

## Автор

Лабораторна робота 1 - ООП на C#
Лабораторна робота 2 - Асинхронне програмування та багатопотоковість

