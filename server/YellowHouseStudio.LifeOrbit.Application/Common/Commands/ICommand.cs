using MediatR;

namespace YellowHouseStudio.LifeOrbit.Application.Common.Commands;

/// <summary>
/// Marker interface for void commands (commands that don't return a value).
/// Commands implementing this interface automatically get transaction behavior.
/// </summary>
public interface ICommand : IRequest { }

/// <summary>
/// Marker interface for commands that return a value.
/// Commands implementing this interface automatically get transaction behavior.
/// </summary>
/// <typeparam name="TResponse">The type of response returned by the command</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse> { } 