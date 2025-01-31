namespace OrderPaymentSystem.Orders.Domain.Exceptions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ProblemDetailsAttribute : Attribute
    {
        public required string Type { get; set; }
        public required string Title { get; set; }
        public int StatusCode { get; set; }
        public bool IncludeEntityInfo { get; set; } = true;
    }
}