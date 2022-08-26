namespace WebApi.Mappers.Interfaces
{
    public interface IUpdateMapper<TSource, TDestination>
    {
        void Map(TSource source, TDestination destination);
    }
}
