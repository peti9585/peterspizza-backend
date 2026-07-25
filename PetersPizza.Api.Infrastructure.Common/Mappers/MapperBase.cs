namespace PetersPizza.Api.Infrastructure.Common.Mappers;

public abstract class MapperBase
{
    protected static IEnumerable<TTarget> MapEnumerable<TSource, TTarget>(IEnumerable<TSource> source, Func<TSource, TTarget> mapper)
        => source?.Select(mapper).ToList() ?? [];

    public TTarget MapEnum<TSource, TTarget>(TSource source) 
        where TSource : Enum 
        where TTarget : Enum
    {
        var targetType = typeof(TTarget);
        var enumValue = Convert.ToInt32(source);
        return Enum.IsDefined(targetType, enumValue) 
            ? (TTarget)Enum.ToObject(targetType, enumValue) 
            : throw new ArgumentOutOfRangeException(nameof(source), source, 
                "Failure on enum mapping!");
    }
}