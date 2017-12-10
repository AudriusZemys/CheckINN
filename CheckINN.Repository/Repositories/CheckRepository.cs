using System;
using System.Collections.Generic;
using System.Linq;
using CheckINN.Repository.Contexts;
using CheckINN.Repository.Entities;

namespace CheckINN.Repository.Repositories
{
    public class CheckRepository : IRepository<Check>
    {
        private readonly Func<ReceiptsContext> _contextFactory;

        public CheckRepository() {}

        public CheckRepository(Func<ReceiptsContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public virtual void Save(Check check)
        {
            using (var context = _contextFactory.Invoke())
            {
                context.Checks.Add(check);
                context.SaveChanges();
            }
        }

        public virtual void SaveMany(IEnumerable<Check> items)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<Check> GetAll()
        {
            using (var context = _contextFactory.Invoke())
            {
                return context.Checks.Select(check => check).ToList();
            }
        }
    }
}
