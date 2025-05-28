namespace CSOS.Core.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
            :base("The entity was not found")
        {
        }

        public EntityNotFoundException(string message)
         : base(message)
        {
        }
        public EntityNotFoundException(string message, Exception innerException)
          : base(message, innerException)
        {
        }
    }
}
