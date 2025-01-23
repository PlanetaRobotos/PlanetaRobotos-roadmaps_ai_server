namespace CourseAI.Application.Models.WayForPays;

public class CreatePaymentRequest
{
    public string PlanType { get; set; } = string.Empty;
    public decimal PlanPrice { get; set; }
    public string? PlanDescription { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
}