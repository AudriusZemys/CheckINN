namespace CheckINN.Domain.Entities
{
    internal class CheckFooter
    {
        public string CashRegister { get; }

        public CheckFooter(string cashRegister)
        {
            CashRegister = cashRegister;
        }
    }
}