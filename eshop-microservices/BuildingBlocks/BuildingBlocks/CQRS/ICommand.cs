using MediatR;

namespace BuildingBlocks.CQRS
{
    /// <summary>
    /// Marker interface for "write" requests that do not return a result.
    /// Use this for commands that modify state but only confirm completion.
    /// Example: public record DeleteProductCommand(...) : ICommand;
    /// </summary>
    public interface ICommand : ICommand<Unit>
    {
    }

    /// <summary>
    /// Marker interface for "write" requests that return a result.
    /// Use this for commands that modify state and return meaningful data.
    /// Example: public record CreateProductCommand(...) : ICommand<Guid>;
    /// </summary>
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}

// Interface            |  Use Case                                  |  Example               |  Returns                  
// ---------------------+--------------------------------------------+------------------------+---------------------------
// ICommand<TResponse>  |  Use when the command has a meaningful     |  CreateProductCommand  |  Guid, DTO, etc.          
//                      |  result to return, such as an ID or a DTO. |                        |                           
// ICommand             |  Use when the command only performs an     |  DeleteProductCommand  |  Unit (nothing meaningful)
//                      |  action and does not need to return data.  |                        |                           
