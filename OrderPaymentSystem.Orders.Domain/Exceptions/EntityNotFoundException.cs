namespace OrderPaymentSystem.Orders.Domain.Exceptions
{
    public class EntityNotFoundException(
        string entityName,
        string fieldName,
        object fieldValue) : Exception(FormatMessage(entityName, fieldName, fieldValue))
    {
        public string EntityName { get; } = entityName;
        public string FieldName { get; } = fieldName;
        public object FieldValue { get; } = fieldValue;

        private static string FormatMessage(
            string entityName,
            string fieldName,
            object fieldValue)
        {
            return $"{entityName} with {fieldName} '{fieldValue}' not found.";
        }
    }
}
