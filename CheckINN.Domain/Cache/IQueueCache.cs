using System.Drawing;

namespace CheckINN.Domain.Cache
{
    public interface IQueueCache<T> : ICache<T>
    {
        T Dequeue();
        bool IsEmpty();
    }
}