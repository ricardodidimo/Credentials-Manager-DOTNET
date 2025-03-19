using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;

namespace webapi.Helpers
{
    public static class ApiResponseExtensions
    {
        public static IResult ToApiError(this IEnumerable<IError> errors, int statusCode = 400)
        {
            var errorMessages = errors.Select(e => e.Message).ToList();
            return Results.Json(new ApiResponse<object>(errorMessages), statusCode: statusCode);
        }

        public static IResult ToApiSuccess<T>(this T data, int? statusCode = 200)
        {
            return Results.Json(new ApiResponse<T>(data), statusCode: statusCode);
        }

        public static IResult ToApiNoContent(this Object _)
        {
            return Results.Json(new ApiResponse<object>(), statusCode: 204);
        }
    }
}
