using System.Collections;
using System.Collections.Concurrent;
using System.Text.Json;

namespace School.Common;

public class CrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
{
    // Thread-safe колекція для зберігання даних
    private readonly ConcurrentDictionary<Guid, T> _storage;
    private readonly Func<T, Guid> _idSelector;
    private readonly SemaphoreSlim _fileSemaphore;
    
    public string FilePath { get; set; }

    // Конструктор
    public CrudServiceAsync(Func<T, Guid> idSelector, string filePath = "data.json")
    {
        _storage = new ConcurrentDictionary<Guid, T>();
        _idSelector = idSelector;
        _fileSemaphore = new SemaphoreSlim(1, 1);
        FilePath = filePath;
    }

    // Метод CreateAsync
    public async Task<bool> CreateAsync(T element)
    {
        return await Task.Run(() =>
        {
            if (element == null)
                return false;

            var id = _idSelector(element);
            
            if (_storage.ContainsKey(id))
                return false;

            return _storage.TryAdd(id, element);
        });
    }

    // Метод ReadAsync
    public async Task<T> ReadAsync(Guid id)
    {
        return await Task.Run(() =>
        {
            if (_storage.TryGetValue(id, out var element))
            {
                return element;
            }

            throw new KeyNotFoundException($"Елемент з ID {id} не знайдено");
        });
    }

    // Метод ReadAllAsync
    public async Task<IEnumerable<T>> ReadAllAsync()
    {
        return await Task.Run(() => _storage.Values.AsEnumerable());
    }

    // Метод ReadAllAsync з пагінацією
    public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
    {
        return await Task.Run(() =>
        {
            if (page < 1 || amount < 1)
                throw new ArgumentException("Page and amount must be greater than 0");

            return _storage.Values
                .Skip((page - 1) * amount)
                .Take(amount)
                .ToList();
        });
    }

    // Метод UpdateAsync
    public async Task<bool> UpdateAsync(T element)
    {
        return await Task.Run(() =>
        {
            if (element == null)
                return false;

            var id = _idSelector(element);

            if (!_storage.ContainsKey(id))
                return false;

            _storage[id] = element;
            return true;
        });
    }

    // Метод RemoveAsync
    public async Task<bool> RemoveAsync(T element)
    {
        return await Task.Run(() =>
        {
            if (element == null)
                return false;

            var id = _idSelector(element);

            return _storage.TryRemove(id, out _);
        });
    }

    // Метод SaveAsync - зберігає колекцію у JSON файл
    public async Task<bool> SaveAsync()
    {
        await _fileSemaphore.WaitAsync();
        try
        {
            var items = _storage.Values.ToList();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            
            var json = JsonSerializer.Serialize(items, options);
            await File.WriteAllTextAsync(FilePath, json);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при збереженні: {ex.Message}");
            return false;
        }
        finally
        {
            _fileSemaphore.Release();
        }
    }

    // Метод LoadAsync - завантажує колекцію з JSON файлу
    public async Task<bool> LoadAsync()
    {
        await _fileSemaphore.WaitAsync();
        try
        {
            if (!File.Exists(FilePath))
                return false;

            var json = await File.ReadAllTextAsync(FilePath);
            var items = JsonSerializer.Deserialize<List<T>>(json);
            
            if (items == null)
                return false;

            _storage.Clear();
            foreach (var item in items)
            {
                var id = _idSelector(item);
                _storage.TryAdd(id, item);
            }
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка при завантаженні: {ex.Message}");
            return false;
        }
        finally
        {
            _fileSemaphore.Release();
        }
    }

    // Реалізація IEnumerable<T>
    public IEnumerator<T> GetEnumerator()
    {
        return _storage.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    // Додатковий метод для отримання кількості елементів
    public int Count => _storage.Count;
}

