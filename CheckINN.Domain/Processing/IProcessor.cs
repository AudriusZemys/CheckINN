namespace CheckINN.Domain.Processing
{
    public interface IProcessor<in T>
    {
        bool TryProcess(T item);
    }
}
