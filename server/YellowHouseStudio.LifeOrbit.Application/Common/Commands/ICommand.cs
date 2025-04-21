using MediatR;

namespace YellowHouseStudio.LifeOrbit.Application.Common.Commands;

/// <summary>
/// Marker interface for commands that need transaction behavior and return a response.
/// Commands implementing this interface must have a validator if they take any inputs.
/// </summary>
public interface ICommand<TResponse> : IRequest<TResponse> { }

/// <summary>
/// Marker interface for commands that need transaction behavior and don't return a response.
/// Commands implementing this interface must have a validator if they take any inputs.
/// </summary>
public interface ICommand : IRequest { } 