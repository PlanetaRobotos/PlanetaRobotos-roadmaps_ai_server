using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Domain.Context;
using OneOf;

namespace Fleet.Application.Features.Users.Create;

public class UserCreateHandler(AppDbContext dbContext) : IHandler<UserCreateRequest, UserModel>
{
    public async ValueTask<OneOf<UserModel, Error>> Handle(UserCreateRequest command, CancellationToken ct)
    {
        // var user = command.MapToEntity();

        // dbContext.Users.Add(user);
        // await dbContext.SaveChangesAsync(ct);

        // return user.MapToModel();
        return default;
    }
}
