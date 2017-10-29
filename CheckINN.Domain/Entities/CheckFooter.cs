namespace CheckINN.Domain.Entities
{
    public struct CheckFooter
    {
        public string CashRegister { get; }

        public CheckFooter(string cashRegister = null)
        {
            CashRegister = cashRegister;
        }
    }
}