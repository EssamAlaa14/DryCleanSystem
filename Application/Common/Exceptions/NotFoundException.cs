namespace Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, Guid id)
            : base($"Entity '{entityName}' with ID '{id}' was not found.")
        {
        }
    }
}
