

using CourseAI.Application.Services;
using CourseAI.Core.Types;
using CourseAI.Domain.Context;
using Microsoft.EntityFrameworkCore;
using NeerCore.DependencyInjection;
using Z.EntityFramework.Plus;

[Service]
public class PositionService(AppDbContext context) : IPositionService
{
    private const int PositionGap = 1000;

    public async Task<int> GetNextPositionAsync<T>(IQueryable<T> query) 
        where T : class, IOrderable
    {
        var lastPosition = await query.MaxAsync(x => (int?)x.Order) ?? 0;
        return lastPosition + PositionGap;
    }

    public async Task InsertAtPositionAsync<T>(IQueryable<T> query, T item, int? position) 
        where T : class, IOrderable
    {
        if (!position.HasValue)
        {
            item.Order = await GetNextPositionAsync(query);
            return;
        }

        var targetPosition = position.Value * PositionGap;
    
        // Get affected items first
        var itemsToUpdate = await query
            .Where(x => x.Order >= targetPosition)
            .ToListAsync();

        // Update their positions in memory
        foreach (var existingItem in itemsToUpdate)
        {
            existingItem.Order += PositionGap;
        }

        // Set the new item's position
        item.Order = targetPosition;
    }
    
    public async Task ReorderAsync<T>(IQueryable<T> query, Dictionary<Guid, int> newPositions) 
        where T : class, IOrderable
    {
        foreach (var (id, position) in newPositions)
        {
            await query
                .Where(x => EF.Property<Guid>(x, "Id") == id)
                .UpdateAsync(x => new { Order = position * PositionGap });
        }
    }

    public async Task NormalizePositionsAsync<T>(IQueryable<T> query) 
        where T : class, IOrderable
    {
        var items = await query
            .OrderBy(x => x.Order)
            .ToListAsync();

        for (int i = 0; i < items.Count; i++)
        {
            items[i].Order = (i + 1) * PositionGap;
        }

        await context.SaveChangesAsync();
    }

    public async Task MoveItemAsync<T>(IQueryable<T> query, T item, int newPosition) 
        where T : class, IOrderable
    {
        var targetPosition = newPosition * PositionGap;

        await query
            .Where(x => x.Order >= targetPosition)
            .UpdateAsync(x => new { Order = x.Order + PositionGap });

        item.Order = targetPosition;
        await context.SaveChangesAsync();
    }
}
