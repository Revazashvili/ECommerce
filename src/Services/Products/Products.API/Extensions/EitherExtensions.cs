using Contracts;

namespace Products.API.Extensions;

internal static class EitherExtensions
{
    internal static IResult ToResult<TLeft, TRight>(this Either<TLeft, TRight> either) =>
        either.Match(Results.Ok, Results.BadRequest);
}