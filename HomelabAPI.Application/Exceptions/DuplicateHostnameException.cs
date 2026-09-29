namespace HomelabAPI.Application.Exceptions
{
    public class DuplicateHostnameException : Exception
    {
        public string Hostname { get; }

        public DuplicateHostnameException(string hostname)
            : base($"A device with hostname '{hostname}' is already registered.")
        {
            Hostname = hostname;
        }
    }
}
