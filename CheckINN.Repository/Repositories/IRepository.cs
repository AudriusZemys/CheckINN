using System.Collections.Generic;

namespace CheckINN.Repository.Repositories
{
    /// <summary>
    /// Defines an interface for domain to save and obtain
    /// entities to and from the database
    /// </summary>
    /// <typeparam name="T">Operating entity</typeparam>
    public interface IRepository<in T>
    {
        void Save(T item);
        void SaveMany(IEnumerable<T> items);
    }
}