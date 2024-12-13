using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CourseAI.Domain.Internal;

internal sealed class ArrayValueComparer<T>() : ValueComparer<T[]?>(
    (l, r) => l == r || l != null && r != null && l.SequenceEqual(r),
    v => v == null ? 0 : v.GetHashCode()
);