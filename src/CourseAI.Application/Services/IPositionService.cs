using CourseAI.Core.Types;

namespace CourseAI.Application.Services;

public interface IPositionService
{
    Task<int> GetNextPositionAsync<T>(IQueryable<T> query) where T : class, IOrderable;
    Task InsertAtPositionAsync<T>(IQueryable<T> query, T item, int? position) where T : class, IOrderable;
    Task ReorderAsync<T>(IQueryable<T> query, Dictionary<Guid, int> newPositions) where T : class, IOrderable;
    Task NormalizePositionsAsync<T>(IQueryable<T> query) where T : class, IOrderable;
    Task MoveItemAsync<T>(IQueryable<T> query, T item, int newPosition) where T : class, IOrderable;
}