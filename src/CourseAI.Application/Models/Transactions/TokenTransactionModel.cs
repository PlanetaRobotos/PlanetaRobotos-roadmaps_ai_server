namespace CourseAI.Application.Models.Transactions;

public class TokenTransactionModel
{
    public int TransactionId { get; set; }
    public int UserId { get; set; }
    public int Amount { get; set; }
    public string TransactionType { get; set; }
    public DateTime Timestamp { get; set; }
}