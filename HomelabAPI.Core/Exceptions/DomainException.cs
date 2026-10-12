namespace HomelabAPI.Core.Exceptions
{
    public class DomainException : Exception
    {
        public string Field { get; }

        public DomainException(string field, string message) : base(message)
        {
            Field = field;
        }
    }
}
