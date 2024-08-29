namespace DEFRA.NE.BNG.Integration.Domain.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public string CustomErrorMessasge { get; private set; }

        public DataNotFoundException() : base()
        {
        }

        public DataNotFoundException(string message) : base(message)
        {
            CustomErrorMessasge = message;
        }

        public DataNotFoundException(string message, Exception inner) : base(message, inner)
        {
            CustomErrorMessasge = message;
        }

    }
}
