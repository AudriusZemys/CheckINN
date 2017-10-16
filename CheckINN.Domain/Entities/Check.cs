namespace CheckINN.Domain.Entities
{
    class Check
    {
        public Check(CheckHeader checkHeader, CheckBody checkBody, CheckFooter checkFooter)
        {
            CheckHeader = checkHeader;
            CheckBody = checkBody;
            CheckFooter = checkFooter;
        }

        public CheckHeader CheckHeader { get; }
        public CheckBody CheckBody { get; }
        public CheckFooter CheckFooter { get; }
    }
}
