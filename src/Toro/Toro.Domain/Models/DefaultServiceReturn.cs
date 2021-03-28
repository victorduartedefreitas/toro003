namespace Toro.Domain.Models
{
    public class DefaultServiceReturn
    {
        #region Constructors

        public DefaultServiceReturn()
        {
        }

        public DefaultServiceReturn(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        #endregion

        #region Properties

        public bool Success { get; set; }
        public string Message { get; set; }

        #endregion
    }
}
