namespace School.Common;

public class CrudService<T> : ICrudService<T> where T : class
{
    // Колекція для зберігання даних
    private readonly Dictionary<Guid, T> _storage;
    private readonly Func<T, Guid> _idSelector;

    // Конструктор
    public CrudService(Func<T, Guid> idSelector)
    {
        _storage = new Dictionary<Guid, T>();
        _idSelector = idSelector;
    }

    // Метод Create
    public void Create(T element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));

        var id = _idSelector(element);
        
        if (_storage.ContainsKey(id))
            throw new InvalidOperationException($"Елемент з ID {id} вже існує");

        _storage[id] = element;
        Console.WriteLine($"Створено елемент з ID: {id}");
    }

    // Метод Read
    public T Read(Guid id)
    {
        if (_storage.TryGetValue(id, out var element))
        {
            return element;
        }

        throw new KeyNotFoundException($"Елемент з ID {id} не знайдено");
    }

    // Метод ReadAll
    public IEnumerable<T> ReadAll()
    {
        return _storage.Values;
    }

    // Метод Update
    public void Update(T element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));

        var id = _idSelector(element);

        if (!_storage.ContainsKey(id))
            throw new KeyNotFoundException($"Елемент з ID {id} не знайдено для оновлення");

        _storage[id] = element;
        Console.WriteLine($"Оновлено елемент з ID: {id}");
    }

    // Метод Remove
    public void Remove(T element)
    {
        if (element == null)
            throw new ArgumentNullException(nameof(element));

        var id = _idSelector(element);

        if (!_storage.Remove(id))
            throw new KeyNotFoundException($"Елемент з ID {id} не знайдено для видалення");

        Console.WriteLine($"Видалено елемент з ID: {id}");
    }

    // Статичний метод
    public static int GetCount(CrudService<T> service)
    {
        return service._storage.Count;
    }
}

