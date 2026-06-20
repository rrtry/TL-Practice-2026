using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Database.Comparers;

public static class ListStringComparer
{
    public static readonly ValueComparer<List<string>> Instance = new(
        ( c1, c2 ) => ( c1 == null && c2 == null ) || ( c1 != null && c2 != null && c1.SequenceEqual( c2 ) ),
        c => c == null ? 0 : c.Aggregate( 0, ( acc, item ) => HashCode.Combine( acc, item.GetHashCode() ) ),
        c => c == null ? new List<string>() : c.ToList()
    );
}