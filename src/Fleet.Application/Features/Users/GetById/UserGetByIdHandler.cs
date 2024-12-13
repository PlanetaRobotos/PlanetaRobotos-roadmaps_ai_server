using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Domain.Context;
using Fleet.Domain.Entities.Identity;
using Mapster;
using OneOf;
using Error = Fleet.Application.Models.Error;

namespace Fleet.Application.Features.Users.GetById;

public class UserGetByIdHandler(AppDbContext dbContext) : IHandler<UserGetByIdRequest, UserModel>
{
    public async ValueTask<OneOf<UserModel, Error>> Handle(UserGetByIdRequest request, CancellationToken ct)
    {
        var user = await dbContext.Users.FindAsync([request.Id,], ct);

        if (user is null)
        {
            return Error.NotFound<User>();
        }

        return user.Adapt<UserModel>();
        // return default;
    }
}
