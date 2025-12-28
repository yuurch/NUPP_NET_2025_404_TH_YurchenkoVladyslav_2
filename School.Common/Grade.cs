namespace School.Common;

// Делегат для подій
public delegate void GradeEventHandler(Grade grade);

public class Grade
{
    // Події
    public static event GradeEventHandler? GradeAssigned;

    // Властивості
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public int Score { get; set; }

    // Конструктор за замовчуванням
    public Grade()
    {
        Id = Guid.NewGuid();
        StudentId = Guid.Empty;
        CourseId = Guid.Empty;
        Score = 0;
    }

    // Конструктор з параметрами
    public Grade(Guid studentId, Guid courseId, int score)
    {
        Id = Guid.NewGuid();
        StudentId = studentId;
        CourseId = courseId;
        Score = score;
    }

    // Метод
    public void Assign()
    {
        Console.WriteLine($"Оцінку {Score} виставлено");
        // Події
        GradeAssigned?.Invoke(this);
    }

    // Метод
    public string GetLetterGrade()
    {
        return Score switch
        {
            >= 90 => "A",
            >= 80 => "B",
            >= 70 => "C",
            >= 60 => "D",
            _ => "F"
        };
    }

    // Метод
    public bool IsPassing()
    {
        return Score >= 60;
    }

    // Статичний метод
    public static double CalculateAverageScore(IEnumerable<Grade> grades)
    {
        var gradeList = grades.ToList();
        if (!gradeList.Any()) return 0;
        return gradeList.Average(g => g.Score);
    }

    public override string ToString()
    {
        return $"Id: {Id}, StudentId: {StudentId}, CourseId: {CourseId}, Score: {Score} ({GetLetterGrade()})";
    }
}

