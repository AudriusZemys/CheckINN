using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace CheckINN.Domain.Cache
{
    /// <summary>
    /// Thread safe cache that works on a queue principle.
    /// Queue because we want to process oldest images first.
    /// </summary>
    public class BitmapQueueCache : IBitmapQueueCache
    {
        private readonly object _lock;
        private readonly Queue<Bitmap> _queue;

        public BitmapQueueCache()
        {
            _lock = new object();
            _queue = new Queue<Bitmap>();
        }

        public void Put(Bitmap item)
        {
            lock (_lock)
            {
                _queue.Enqueue(item);
            }
        }

        public IEnumerator<Bitmap> GetEnumerator()
        {
            lock (_lock)
            {
                return _queue.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Bitmap Dequeue()
        {
            lock (_lock)
            {
                return _queue.Dequeue();
            }
        }

        public bool IsEmpty()
        {
            lock (_lock)
            {
                return _queue.Count == 0;
            }
        }
    }
}
