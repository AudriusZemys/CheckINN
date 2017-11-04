namespace CheckINN.WebApi.Entities
{
    class Status
    {
        public Status(int code, string message)
        {
            Code = code;
            Message = message;
        }

        private int Code { get; }
        private string Message { get; }
    }
}
