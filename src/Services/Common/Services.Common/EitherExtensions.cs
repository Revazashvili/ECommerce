using Contracts;
using Microsoft.AspNetCore.Http;

namespace Services.Common;

public static class EitherExtensions
{
    public static IResult ToResult<TLeft, TRight>(this Either<TLeft, TRight> either) =>
        either.Match(Results.Ok, Results.BadRequest);
}