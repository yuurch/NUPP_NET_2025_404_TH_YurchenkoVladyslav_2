namespace School.Common;

public class Course
{
    // Статичне поле - загальна кількість курсів
    private static int _totalCoursesCreated = 0;

    // Властивості
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Credits { get; set; }
    public Guid TeacherId { get; set; }

    // Статичний конструктор
    static Course()
    {
        _totalCoursesCreated = 0;
        Console.WriteLine("Course клас ініціалізовано");
    }

    // Конструктор за замовчуванням
    public Course()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
        Credits = 0;
        TeacherId = Guid.Empty;
        _totalCoursesCreated++;
    }

    // Конструктор з параметрами
    public Course(string name, int credits, Guid teacherId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Credits = credits;
        TeacherId = teacherId;
        _totalCoursesCreated++;
    }

    // Метод
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Name) && Credits > 0 && TeacherId != Guid.Empty;
    }

    // Метод
    public string GetCourseInfo()
    {
        return $"{Name} ({Credits} кредитів)";
    }

    // Статичний метод
    public static int GetTotalCoursesCreated()
    {
        return _totalCoursesCreated;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Credits: {Credits}, TeacherId: {TeacherId}";
    }
}

