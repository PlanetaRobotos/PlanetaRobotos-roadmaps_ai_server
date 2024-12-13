using CourseAI.Application.Core;
using CourseAI.Application.Models;

namespace CourseAI.Application.Features.Users.GetById;

public class UserGetByIdRequest : IRequestModel<UserModel>
{
    public long Id { get; init; }
}
