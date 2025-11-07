
using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool IsSuccess);
    public class DeleteProductCommandHandler(IDocumentSession session)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
        {
            public DeleteProductCommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required");
            }
        }

        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            session.Delete<Product>(command.Id);
            await session.SaveChangesAsync(cancellationToken);

            return new DeleteProductResult(true);
        }
    }
}
