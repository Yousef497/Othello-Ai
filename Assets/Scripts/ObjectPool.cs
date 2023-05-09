using System;
using System.Collections.Concurrent;

public class ObjectPool<T>
{
    private readonly ConcurrentBag<T> objects;
    private readonly Func<T> objectGenerator;

    public ObjectPool(Func<T> objectGenerator)
    {
        if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
        this.objects = new ConcurrentBag<T>();
        this.objectGenerator = objectGenerator;
    }

    public T GetObject()
    {
        T item;
        if (this.objects.TryTake(out item)) return item;
        return this.objectGenerator();
    }

    public void PutObject(T item)
    {
        this.objects.Add(item);
    }
}
