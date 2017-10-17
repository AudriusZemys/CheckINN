using System;
using CheckINN.Domain.Entities;

namespace CheckINN.Domain.Processing
{
    public interface ICheckProcessor : IProcessor<Check>
    {
    }

    public class BasicCheckProcessor : ICheckProcessor {
        public bool TryProcess(Check item)
        {
            bool result = true;
            try
            {

            }
            catch (Exception ex)
            {
                result = false;
                Console.Write(ex.Message);
            }
            return result;
        }
    }
}