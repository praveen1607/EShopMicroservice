using MediatR;

namespace BuildingBlocks.CQRS
{
    /// <summary>
    /// Marker interface for "read" requests (queries) that fetch data.
    /// Use this for queries that return a non-null result.
    /// Example: public record GetProductQuery(Guid Id) : IQuery<Product>;
    /// </summary>
    /// <typeparam name="TResponse">The type of the response, which must be non-null.</typeparam>
    public interface IQuery<out TResponse> : IRequest<TResponse>
        where TResponse : notnull
    {
    }
}
