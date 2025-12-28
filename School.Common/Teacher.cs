namespace School.Common;

public class Teacher : Person
{
    // Статичне поле - мінімальна зарплата
    public static readonly decimal MinimumSalary = 15000m;

    // Властивості
    public string Department { get; set; }
    public string Position { get; set; }
    public decimal Salary { get; set; }

    // Конструктор за замовчуванням
    public Teacher() : base()
    {
        Department = string.Empty;
        Position = string.Empty;
        Salary = MinimumSalary;
    }

    // Конструктор з параметрами
    public Teacher(string firstName, string lastName, DateTime dateOfBirth, string department, string position, decimal salary)
        : base(firstName, lastName, dateOfBirth)
    {
        Department = department;
        Position = position;
        Salary = salary < MinimumSalary ? MinimumSalary : salary;
    }

    // Метод
    public void IncreaseSalary(decimal percentage)
    {
        if (percentage > 0)
        {
            Salary += Salary * (percentage / 100);
            Console.WriteLine($"Зарплату викладача {GetFullName()} збільшено на {percentage}%");
        }
    }

    // Метод
    public bool IsSeniorTeacher()
    {
        return Position.Contains("професор", StringComparison.OrdinalIgnoreCase) ||
               Position.Contains("доцент", StringComparison.OrdinalIgnoreCase);
    }

    // Статичний метод
    public static decimal CalculateAnnualSalary(decimal monthlySalary)
    {
        return monthlySalary * 12;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, Department: {Department}, Position: {Position}, Salary: {Salary:N2} ₴";
    }

    // Статичний метод для створення нового об'єкта із згенерованими даними
    public static Teacher CreateNew()
    {
        var random = new Random();
        
        // Списки для генерації імен
        string[] firstNames = { "Іван", "Петро", "Марія", "Олена", "Андрій", "Наталія", "Сергій", "Тетяна", "Володимир", "Оксана", "Дмитро", "Юлія", "Олександр", "Ірина", "Віктор", "Світлана", "Михайло", "Людмила", "Василь", "Анна" };
        string[] lastNames = { "Петренко", "Коваленко", "Шевченко", "Бондаренко", "Мельник", "Ткаченко", "Кравченко", "Іваненко", "Морозов", "Павленко", "Сидоренко", "Литвиненко", "Марченко", "Savchenko", "Попов", "Демченко", "Григоренко", "Романенко", "Федоренко", "Давиденко" };
        
        // Списки департаментів та посад
        string[] departments = { "Математика", "Фізика", "Інформатика", "Хімія", "Біологія", "Історія", "Література", "Іноземні мови", "Економіка", "Філософія" };
        string[] positions = { "Професор", "Доцент", "Викладач", "Старший викладач", "Асистент" };
        
        // Генерація випадкових даних
        string firstName = firstNames[random.Next(firstNames.Length)];
        string lastName = lastNames[random.Next(lastNames.Length)];
        DateTime dateOfBirth = DateTime.Now.AddYears(-random.Next(25, 66)); // Вік від 25 до 65 років
        string department = departments[random.Next(departments.Length)];
        string position = positions[random.Next(positions.Length)];
        decimal salary = random.Next(15000, 50001); // Зарплата від 15000 до 50000
        
        return new Teacher(firstName, lastName, dateOfBirth, department, position, salary);
    }
}

