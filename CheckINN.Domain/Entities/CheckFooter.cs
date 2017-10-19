namespace CheckINN.Domain.Entities
{
    public class CheckFooter
    {
        public string CashRegister { get; }

        public CheckFooter(string cashRegister)
        {
            CashRegister = cashRegister;
        }
    }
}