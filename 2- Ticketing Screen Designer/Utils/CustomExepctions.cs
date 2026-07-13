using System;

namespace Ticketing_Screen_Designer.Utils
{

    public class DataAccessException : Exception
    {
        public DataAccessException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class DuplicateRecordException : DataAccessException
    {
        public DuplicateRecordException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class ExcessiveScreenActivationException : DataAccessException
    {
        public ExcessiveScreenActivationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}