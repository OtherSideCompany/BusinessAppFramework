namespace BusinessAppFramework.Application.Exceptions
{
    public class DomainException : Exception
    {
        public string ErrorKey { get; }

        public DomainException(string errorKey)
        {
            ErrorKey = errorKey;
        }       
    }
}
