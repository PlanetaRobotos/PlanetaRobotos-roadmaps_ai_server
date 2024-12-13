using Fleet.Application.Core;
using Fleet.Application.Models;

namespace Fleet.Application.Features.Users.GetById;

public class UserGetByIdRequest : IRequestModel<UserModel>
{
    public long Id { get; init; }
}
