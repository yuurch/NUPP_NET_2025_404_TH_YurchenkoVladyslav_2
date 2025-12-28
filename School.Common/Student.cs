namespace School.Common;

public class Student : Person
{
    // Події
    public static event PersonEventHandler? StudentEnrolled;

    // Властивості
    public string StudentNumber { get; set; }
    public int Year { get; set; }
    public double GPA { get; set; }

    // Конструктор за замовчуванням
    public Student() : base()
    {
        StudentNumber = string.Empty;
        Year = 1;
        GPA = 0.0;
    }

    // Конструктор з параметрами
    public Student(string firstName, string lastName, DateTime dateOfBirth, string studentNumber, int year, double gpa)
        : base(firstName, lastName, dateOfBirth)
    {
        StudentNumber = studentNumber;
        Year = year;
        GPA = gpa;
    }

    // Метод
    public void Enroll()
    {
        Console.WriteLine($"Студент {GetFullName()} зараховано");
        // Події
        StudentEnrolled?.Invoke(this);
    }

    // Метод
    public bool IsHonorStudent()
    {
        return GPA >= 4.5;
    }

    // Метод
    public string GetAcademicStatus()
    {
        return GPA switch
        {
            >= 4.5 => "Відмінник",
            >= 3.5 => "Хорошист",
            >= 2.0 => "Задовільно",
            _ => "Незадовільно"
        };
    }

    public override string ToString()
    {
        return $"{base.ToString()}, Student#: {StudentNumber}, Year: {Year}, GPA: {GPA:F2}, Status: {GetAcademicStatus()}";
    }
}

