using CourseAI.Application.Core;

namespace CourseAI.Application.Features.Tokens;

public class RefillTokensRequest : IRequestModel
{
    public long UserId { get; set; }
    public int Amount { get; set; }
}