using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Behaviors
{
    /// <summary>
    /// ValidationBehavior is a MediatR pipeline behavior that ensures commands are validated
    /// before they are passed to their respective handlers.
    /// </summary>
    /// <typeparam name="Trequest">The type of the command being validated. Must implement ICommand<TResponse>.</typeparam>
    /// <typeparam name="TResponse">The type of the response returned by the command handler.</typeparam>
    public class ValidationBehavior<Trequest, TResponse> (IEnumerable<IValidator<Trequest>> validators) : IPipelineBehavior<Trequest, TResponse>
        where Trequest : ICommand<TResponse>
        // Why ICommand? We want to apply validation only to commands, not queries.
        where TResponse : notnull
    {
        /// <summary>
        /// Handles the validation of the command before passing it to the next behavior or handler.
        /// </summary>
        /// <param name="command">The command object to be validated.</param>
        /// <param name="next">The next delegate in the pipeline, which represents the command handler.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The response from the next behavior or handler if validation passes.</returns>
        /// <exception cref="ValidationException">Thrown if any validation errors are found.</exception>
        public async Task<TResponse> Handle(Trequest command, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Create a validation context for the command.
            // This context is used to pass the command and any additional metadata to the validators.
            var context = new ValidationContext<Trequest>(command);

            // Execute all validators for the command concurrently and collect their results.
            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // Aggregate all validation errors from the results.
            var errors = validationResults
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .ToList();

            // If there are any validation errors, throw a ValidationException.
            if (errors.Any())
            {
                throw new FluentValidation.ValidationException(errors); // You can customize this to include all errors.
            }

            // If validation passes, proceed to the next behavior or handler in the pipeline.
            return await next();
        }
    }
}
