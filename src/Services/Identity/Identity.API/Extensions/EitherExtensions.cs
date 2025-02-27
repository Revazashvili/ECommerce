using Contracts;

namespace Identity.API.Extensions;

internal static class EitherExtensions
{
    public static IResult ToResult<TLeft, TRight>(this Either<TLeft, TRight> either) =>
        either.Match(Results.Ok, Results.BadRequest);
}