using School.Common;
using Xunit;

namespace School.Tests;

public class CrudServiceAsyncTests : IDisposable
{
    private readonly string _testFilePath = "test_data.json";
    private readonly CrudServiceAsync<Teacher> _service;

    public CrudServiceAsyncTests()
    {
        _service = new CrudServiceAsync<Teacher>(t => t.Id, _testFilePath);
    }

    public void Dispose()
    {
        // Очищення тестових файлів
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    // ========================================================================
    // ТЕСТИ CRUD ОПЕРАЦІЙ
    // ========================================================================

    [Fact]
    public async Task CreateAsync_Should_Add_New_Element()
    {
        // Arrange
        var teacher = new Teacher("Іван", "Петренко", new DateTime(1980, 5, 15), "Математика", "Професор", 25000m);

        // Act
        var result = await _service.CreateAsync(teacher);

        // Assert
        Assert.True(result);
        Assert.Equal(1, _service.Count);
    }

    [Fact]
    public async Task CreateAsync_Should_Return_False_For_Duplicate_Id()
    {
        // Arrange
        var teacher1 = new Teacher("Іван", "Петренко", new DateTime(1980, 5, 15), "Математика", "Професор", 25000m);
        var teacher2 = new Teacher("Марія", "Коваленко", new DateTime(1985, 8, 20), "Фізика", "Доцент", 20000m);
        teacher2.Id = teacher1.Id; // Той самий ID

        // Act
        await _service.CreateAsync(teacher1);
        var result = await _service.CreateAsync(teacher2);

        // Assert
        Assert.False(result);
        Assert.Equal(1, _service.Count);
    }

    [Fact]
    public async Task CreateAsync_Should_Return_False_For_Null_Element()
    {
        // Act
        var result = await _service.CreateAsync(null!);

        // Assert
        Assert.False(result);
        Assert.Equal(0, _service.Count);
    }

    [Fact]
    public async Task ReadAsync_Should_Return_Existing_Element()
    {
        // Arrange
        var teacher = new Teacher("Іван", "Петренко", new DateTime(1980, 5, 15), "Математика", "Професор", 25000m);
        await _service.CreateAsync(teacher);

        // Act
        var result = await _service.ReadAsync(teacher.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(teacher.Id, result.Id);
        Assert.Equal(teacher.FirstName, result.FirstName);
    }

    [Fact]
    public async Task ReadAsync_Should_Throw_Exception_For_NonExistent_Id()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.ReadAsync(nonExistentId));
    }

    [Fact]
    public async Task ReadAllAsync_Should_Return_All_Elements()
    {
        // Arrange
        var teacher1 = new Teacher("Іван", "Петренко", new DateTime(1980, 5, 15), "Математика", "Професор", 25000m);
        var teacher2 = new Teacher("Марія", "Коваленко", new DateTime(1985, 8, 20), "Фізика", "Доцент", 20000m);
        var teacher3 = new Teacher("Петро", "Сидоренко", new DateTime(1975, 3, 10), "Хімія", "Викладач", 18000m);

        await _service.CreateAsync(teacher1);
        await _service.CreateAsync(teacher2);
        await _service.CreateAsync(teacher3);

        // Act
        var result = await _service.ReadAllAsync();

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Existing_Element()
    {
        // Arrange
        var teacher = new Teacher("Іван", "Петренко", new DateTime(1980, 5, 15), "Математика", "Професор", 25000m);
        await _service.CreateAsync(teacher);

        // Act
        teacher.Salary = 30000m;
        var result = await _service.UpdateAsync(teacher);
        var updated = await _service.ReadAsync(teacher.Id);

        // Assert
        Assert.True(result);
        Assert.Equal(30000m, updated.Salary);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_False_For_NonExistent_Element()
    {
        // Arrange
        var teacher = new Teacher("Іван", "Петренко", new DateTime(1980, 5, 15), "Математика", "Професор", 25000m);

        // Act
        var result = await _service.UpdateAsync(teacher);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_False_For_Null_Element()
    {
        // Act
        var result = await _service.UpdateAsync(null!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_Existing_Element()
    {
        // Arrange
        var teacher = new Teacher("Іван", "Петренко", new DateTime(1980, 5, 15), "Математика", "Професор", 25000m);
        await _service.CreateAsync(teacher);

        // Act
        var result = await _service.RemoveAsync(teacher);

        // Assert
        Assert.True(result);
        Assert.Equal(0, _service.Count);
    }

    [Fact]
    public async Task RemoveAsync_Should_Return_False_For_NonExistent_Element()
    {
        // Arrange
        var teacher = new Teacher("Іван", "Петренко", new DateTime(1980, 5, 15), "Математика", "Професор", 25000m);

        // Act
        var result = await _service.RemoveAsync(teacher);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task RemoveAsync_Should_Return_False_For_Null_Element()
    {
        // Act
        var result = await _service.RemoveAsync(null!);

        // Assert
        Assert.False(result);
    }

    // ========================================================================
    // ТЕСТИ ПАГІНАЦІЇ
    // ========================================================================

    [Fact]
    public async Task ReadAllAsync_WithPagination_Should_Return_Correct_Page()
    {
        // Arrange
        for (int i = 0; i < 25; i++)
        {
            var teacher = Teacher.CreateNew();
            await _service.CreateAsync(teacher);
        }

        // Act
        var page1 = await _service.ReadAllAsync(1, 10);
        var page2 = await _service.ReadAllAsync(2, 10);
        var page3 = await _service.ReadAllAsync(3, 10);

        // Assert
        Assert.Equal(10, page1.Count());
        Assert.Equal(10, page2.Count());
        Assert.Equal(5, page3.Count());
    }

    [Fact]
    public async Task ReadAllAsync_WithPagination_Should_Throw_For_Invalid_Page()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.ReadAllAsync(0, 10));
    }

    [Fact]
    public async Task ReadAllAsync_WithPagination_Should_Throw_For_Invalid_Amount()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.ReadAllAsync(1, 0));
    }

    [Fact]
    public async Task ReadAllAsync_WithPagination_Should_Return_Empty_For_OutOfRange_Page()
    {
        // Arrange
        var teacher = new Teacher("Іван", "Петренко", new DateTime(1980, 5, 15), "Математика", "Професор", 25000m);
        await _service.CreateAsync(teacher);

        // Act
        var result = await _service.ReadAllAsync(10, 10);

        // Assert
        Assert.Empty(result);
    }

    // ========================================================================
    // ТЕСТИ СЕРІАЛІЗАЦІЇ
    // ========================================================================

    [Fact]
    public async Task SaveAsync_Should_Create_File()
    {
        // Arrange
        var teacher = new Teacher("Іван", "Петренко", new DateTime(1980, 5, 15), "Математика", "Професор", 25000m);
        await _service.CreateAsync(teacher);

        // Act
        var result = await _service.SaveAsync();

        // Assert
        Assert.True(result);
        Assert.True(File.Exists(_testFilePath));
    }

    [Fact]
    public async Task SaveAsync_Should_Save_All_Data()
    {
        // Arrange
        for (int i = 0; i < 10; i++)
        {
            var teacher = Teacher.CreateNew();
            await _service.CreateAsync(teacher);
        }

        // Act
        await _service.SaveAsync();
        var newService = new CrudServiceAsync<Teacher>(t => t.Id, _testFilePath);
        await newService.LoadAsync();

        // Assert
        Assert.Equal(_service.Count, newService.Count);
    }

    [Fact]
    public async Task LoadAsync_Should_Return_False_For_NonExistent_File()
    {
        // Arrange
        var service = new CrudServiceAsync<Teacher>(t => t.Id, "nonexistent.json");

        // Act
        var result = await service.LoadAsync();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task LoadAsync_Should_Load_Data_From_File()
    {
        // Arrange
        var teacher1 = new Teacher("Іван", "Петренко", new DateTime(1980, 5, 15), "Математика", "Професор", 25000m);
        var teacher2 = new Teacher("Марія", "Коваленко", new DateTime(1985, 8, 20), "Фізика", "Доцент", 20000m);
        
        await _service.CreateAsync(teacher1);
        await _service.CreateAsync(teacher2);
        await _service.SaveAsync();

        // Act
        var newService = new CrudServiceAsync<Teacher>(t => t.Id, _testFilePath);
        var result = await newService.LoadAsync();
        var loadedTeachers = await newService.ReadAllAsync();

        // Assert
        Assert.True(result);
        Assert.Equal(2, loadedTeachers.Count());
    }

    // ========================================================================
    // ТЕСТИ БАГАТОПОТОКОВОСТІ
    // ========================================================================

    [Fact]
    public async Task CreateAsync_Should_Be_ThreadSafe()
    {
        // Arrange
        var tasks = new List<Task<bool>>();

        // Act
        for (int i = 0; i < 100; i++)
        {
            var teacher = Teacher.CreateNew();
            tasks.Add(_service.CreateAsync(teacher));
        }

        await Task.WhenAll(tasks);

        // Assert
        Assert.Equal(100, _service.Count);
    }

    [Fact]
    public async Task ConcurrentOperations_Should_Be_ThreadSafe()
    {
        // Arrange
        var teachers = new List<Teacher>();
        for (int i = 0; i < 50; i++)
        {
            var teacher = Teacher.CreateNew();
            teachers.Add(teacher);
            await _service.CreateAsync(teacher);
        }

        // Act - паралельні операції: читання, оновлення, видалення
        var tasks = new List<Task>();
        
        // Читання
        for (int i = 0; i < 20; i++)
        {
            tasks.Add(Task.Run(async () => await _service.ReadAllAsync()));
        }
        
        // Оновлення
        for (int i = 0; i < 10; i++)
        {
            int index = i;
            tasks.Add(Task.Run(async () =>
            {
                if (index < teachers.Count)
                {
                    teachers[index].Salary += 1000;
                    await _service.UpdateAsync(teachers[index]);
                }
            }));
        }
        
        // Видалення
        for (int i = 0; i < 5; i++)
        {
            int index = i;
            tasks.Add(Task.Run(async () =>
            {
                if (index < teachers.Count)
                {
                    await _service.RemoveAsync(teachers[index]);
                }
            }));
        }

        await Task.WhenAll(tasks);

        // Assert
        Assert.Equal(45, _service.Count); // 50 - 5 видалених
    }

    [Fact]
    public async Task SaveAsync_Should_Be_ThreadSafe()
    {
        // Arrange
        for (int i = 0; i < 10; i++)
        {
            var teacher = Teacher.CreateNew();
            await _service.CreateAsync(teacher);
        }

        // Act - множинні паралельні збереження
        var tasks = new List<Task<bool>>();
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(_service.SaveAsync());
        }

        var results = await Task.WhenAll(tasks);

        // Assert
        Assert.All(results, result => Assert.True(result));
        Assert.True(File.Exists(_testFilePath));
    }

    // ========================================================================
    // ТЕСТИ IEnumerable
    // ========================================================================

    [Fact]
    public async Task GetEnumerator_Should_Enumerate_All_Elements()
    {
        // Arrange
        for (int i = 0; i < 5; i++)
        {
            var teacher = Teacher.CreateNew();
            await _service.CreateAsync(teacher);
        }

        // Act
        var count = 0;
        foreach (var teacher in _service)
        {
            count++;
            Assert.NotNull(teacher);
        }

        // Assert
        Assert.Equal(5, count);
    }

    [Fact]
    public async Task GetEnumerator_Should_Work_With_LINQ()
    {
        // Arrange
        for (int i = 0; i < 10; i++)
        {
            var teacher = Teacher.CreateNew();
            await _service.CreateAsync(teacher);
        }

        // Act
        var highSalaryTeachers = _service.Where(t => t.Salary > 30000).ToList();

        // Assert
        Assert.NotNull(highSalaryTeachers);
        Assert.All(highSalaryTeachers, t => Assert.True(t.Salary > 30000));
    }

    // ========================================================================
    // ТЕСТИ CreateNew() МЕТОДУ
    // ========================================================================

    [Fact]
    public void CreateNew_Should_Generate_Valid_Teacher()
    {
        // Act
        var teacher = Teacher.CreateNew();

        // Assert
        Assert.NotNull(teacher);
        Assert.NotEqual(Guid.Empty, teacher.Id);
        Assert.NotEmpty(teacher.FirstName);
        Assert.NotEmpty(teacher.LastName);
        Assert.NotEmpty(teacher.Department);
        Assert.NotEmpty(teacher.Position);
        Assert.True(teacher.Salary >= Teacher.MinimumSalary);
        Assert.True(teacher.CalculateAge() >= 25);
        Assert.True(teacher.CalculateAge() <= 65);
    }

    [Fact]
    public void CreateNew_Should_Generate_Different_Teachers()
    {
        // Act
        var teacher1 = Teacher.CreateNew();
        var teacher2 = Teacher.CreateNew();

        // Assert
        Assert.NotEqual(teacher1.Id, teacher2.Id);
    }
}

