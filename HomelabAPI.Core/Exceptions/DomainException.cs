namespace HomelabAPI.Core.Exceptions
{
    public class DomainException : AppException
    {

        public DomainException(string field, string message) : base(message, ErrorCategory.Validation, ErrorCodes.DomainValidation, field)
        {
        }
    }
}
