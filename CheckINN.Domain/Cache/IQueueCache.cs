using System.Drawing;

namespace CheckINN.Domain.Cache
{
    /// <summary>
    /// A variation of ICache that utilizes Queue pecularities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueueCache<T> : ICache<T>
    {
        /// <summary>
        /// Takes a single item from the beggining of queue
        /// </summary>
        /// <returns>Some item from the queue</returns>
        T Dequeue();

        /// <summary>
        /// Checks if the queue is empty
        /// </summary>
        bool IsEmpty();
    }
}