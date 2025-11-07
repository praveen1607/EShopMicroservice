
namespace Catalog.API.Products.CreateProduct
{
    /// <summary>
    /// Command for creating a new product.
    /// Implements ICommand<CreateProductResult> to follow CQRS and MediatR patterns.
    /// Contains all data required to create a product.
    /// </summary>
    public record CreateProductCommand(
        string Name,
        List<string> Category,
        string Description,
        string ImageFile,
        decimal Price) : ICommand<CreateProductResult>;

    /// <summary>
    /// Result returned after creating a product.
    /// Contains the unique identifier (Id) of the newly created product.
    /// </summary>
    public record CreateProductResult(Guid Id);

    /// <summary>
    /// Handler for CreateProductCommand.
    /// Implements ICommandHandler<CreateProductCommand, CreateProductResult>.
    /// Responsible for processing the command and returning the result.
    /// </summary>
    /// 
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
    public class CreateProductCommandHandler(IDocumentSession session, ILogger<CreateProductCommandHandler> logger) : ICommandHandler<CreateProductCommand, CreateProductResult>
            //public class CreateProductCommandHandler(IDocumentSession session, IValidator<CreateProductCommand> validator) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        /// <summary>
        /// Handles the creation of a new product.
        /// Maps command data to a Product entity and returns a result with a new Guid.
        /// 
        /// Why async and Task<>?
        /// - The `async` keyword allows the method to run asynchronously, enabling non-blocking operations.
        /// - Returning a `Task<CreateProductResult>` ensures compatibility with asynchronous frameworks like MediatR.
        /// - This is particularly useful for I/O-bound operations (e.g., database calls) that may be added later.
        /// - Even if the current implementation is synchronous, using `async` prepares the method for future asynchronous logic.
        /// </summary>
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {

            logger.LogInformation("CreateProductCommandHandler.Handle called with {@Command}", command);
            //var result = await validator.ValidateAsync(command, cancellationToken);
            //var errors = result.Errors.Select(x => x.ErrorMessage).ToList();
            //if (errors.Any())
            //{
            //    throw new ValidationException(errors.FirstOrDefault());
            //}

            // Map the command data to a Product entity.
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            // Persist the product to the database and use the actual product Id.
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            // Simulating asynchronous behavior with Task.FromResult for now.
            return new CreateProductResult(product.Id);
        }
    }
}
