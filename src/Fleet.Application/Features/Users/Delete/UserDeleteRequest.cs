using Fleet.Application.Core;

namespace Fleet.Application.Features.Users.Delete;

public class UserDeleteRequest : IRequestModel
{
    public long Id { get; init; }
}
