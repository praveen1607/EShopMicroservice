namespace Catalog.API.Products.GetProducts
{
    public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);

    public class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            // Ensure PageNumber and PageSize have default values if null
            var pageNumber = query.PageNumber ?? 1;
            var pageSize = query.PageSize ?? 10;

            // Use Marten's ToPagedListAsync for pagination
            var products = await session.Query<Product>()
                .ToPagedListAsync(pageNumber, pageSize, cancellationToken);

            return new GetProductsResult(products);
        }
    }
}
