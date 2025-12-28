namespace School.Common;

// Делегат для подій
public delegate void PersonEventHandler(Person person);

public class Person
{
    // Статичне поле - лічильник створених об'єктів
    private static int _totalPersonsCreated = 0;

    // Властивості
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }

    // Статичний конструктор
    static Person()
    {
        _totalPersonsCreated = 0;
        Console.WriteLine("Person клас ініціалізовано");
    }

    // Конструктор за замовчуванням
    public Person()
    {
        Id = Guid.NewGuid();
        FirstName = string.Empty;
        LastName = string.Empty;
        DateOfBirth = DateTime.Now;
        _totalPersonsCreated++;
    }

    // Конструктор з параметрами
    public Person(string firstName, string lastName, DateTime dateOfBirth)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        _totalPersonsCreated++;
    }

    // Метод
    public int CalculateAge()
    {
        var today = DateTime.Today;
        var age = today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }

    // Метод
    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }

    // Статичний метод
    public static int GetTotalPersonsCreated()
    {
        return _totalPersonsCreated;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {GetFullName()}, Age: {CalculateAge()}";
    }
}

