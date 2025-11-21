namespace Basket.API.Basket.GetBasket
{
    public record GetBasketResponse(ShoppingCart Cart);
    public class GetBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var query = new GetBasketQuery(userName);
                var result = await sender.Send(query);
                return Results.Ok(result.Cart);
            })
            .WithName("GetBasket")
            .Produces<ShoppingCart>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Basket")
            .WithDescription("Get basket by user name");
        }
    }
}
