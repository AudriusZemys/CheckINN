using System;
using CheckINN.Domain.Cache;
using CheckINN.Domain.Entities;

namespace CheckINN.Domain.Processing
{
    public class BasicCheckProcessor : ICheckProcessor
    {
        private readonly ICheckCache _cache;

        public BasicCheckProcessor(ICheckCache cache)
        {
            _cache = cache;
        }

        public bool TryProcess(Check item)
        {
            bool result = true;
            try
            {
                _cache.Put(item);
            }
            catch (Exception e)
            {
                result = false;
                Console.Write(e.Message);
            }
            return result;
        }
    }

}