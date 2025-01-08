using CourseAI.Application.Core;

namespace CourseAI.Application.Features.Purchases;

public class UpdateUsersTokensRequest: IRequestModel
{
    public int TokensAmount { get; set; }
}