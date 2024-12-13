using Fleet.Application.Core;
using Fleet.Application.Models;
using Fleet.Domain.Context;
using Fleet.Domain.Entities.Identity;
using Mediator;
using OneOf;

namespace Fleet.Application.Features.TableSettings.Update;

public class TableSettingsUpdateHandler(AppDbContext dbContext) : IHandler<UpdateTableSettingsRequest>
{
    public async ValueTask<OneOf<Unit, Error>> Handle(UpdateTableSettingsRequest request, CancellationToken ct)
    {
        var tableSettings = await dbContext.TableSettings.FindAsync([request.Id, request.TableSettingsName,], ct);

        // Create
        if (tableSettings is null)
        {
            tableSettings = new UserTableSettings
            {
                UserId = request.Id,
                TableName = request.TableSettingsName,
                Columns = request.Columns!,
            };

            await dbContext.TableSettings.AddAsync(tableSettings, ct);
            await dbContext.SaveChangesAsync(ct);
            return Unit.Value;
        }

        // Update
        tableSettings.Columns = request.Columns!;
        await dbContext.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
