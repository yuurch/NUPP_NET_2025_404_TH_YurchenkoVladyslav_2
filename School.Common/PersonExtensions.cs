namespace School.Common;

// Метод розширення
public static class PersonExtensions
{
    // Метод розширення для Person
    public static string GetInitials(this Person person)
    {
        if (string.IsNullOrWhiteSpace(person.FirstName) || string.IsNullOrWhiteSpace(person.LastName))
            return string.Empty;

        return $"{person.FirstName[0]}.{person.LastName[0]}.";
    }

    // Метод розширення для Student
    public static string GetStudentSummary(this Student student)
    {
        return $"{student.GetFullName()} ({student.StudentNumber}) - Рік {student.Year}, GPA: {student.GPA:F2}";
    }

    // Метод розширення для string
    public static string ToTitleCase(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        var words = text.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length > 0)
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
        }
        return string.Join(" ", words);
    }
}

