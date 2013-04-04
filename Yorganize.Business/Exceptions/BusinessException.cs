using System;

namespace Yorganize.Business.Exceptions
{
    public class BusinessException : Exception
    {

        public Guid CorrelationID { get; set; }

        private readonly ExceptionType _type;
        public ExceptionType Type
        {
            get
            {
                return _type;
            }
        }

        public BusinessException(string message, Exception innerException, ExceptionType type)
            : base(message, innerException)
        {
            _type = type;
            CorrelationID = Guid.NewGuid();
        }

        public BusinessException(string message)
            : this(message, null, ExceptionType.ERROR)
        {

        }

        public BusinessException(string message, Exception innerException)
            : this(message, innerException, ExceptionType.ERROR)
        {

        }

    }
}
