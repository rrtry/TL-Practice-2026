using System.Net;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware( RequestDelegate next )
    {
        _next = next;
    }

    public async Task InvokeAsync( HttpContext context )
    {
        try
        {
            await _next( context );
        }
        catch ( Exception ex )
        {
            await HandleExceptionAsync( context, ex );
        }
    }

    private Task HandleExceptionAsync( HttpContext context, Exception exception )
    {
        var (statusCode, title) = exception switch
        {
            PropertyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            ReservationNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            RoomTypeNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            RoomTypeMismatchException => (HttpStatusCode.BadRequest, exception.Message),
            RoomTypeHasReservationsException => (HttpStatusCode.BadRequest, exception.Message),
            InvalidDateRangeException => (HttpStatusCode.BadRequest, exception.Message),
            InvalidArrivalDateException => (HttpStatusCode.BadRequest, exception.Message),
            InvalidArrivalTimeException => (HttpStatusCode.BadRequest, exception.Message),
            GuestCountOutOfRangeException => (HttpStatusCode.BadRequest, exception.Message),
            NoAvailableRoomsException => (HttpStatusCode.Conflict, exception.Message),

            KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
            InvalidOperationException => (HttpStatusCode.Conflict, exception.Message),
            PropertyHasExistingReservationsException => (HttpStatusCode.Conflict, exception.Message),

            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = ( int )statusCode;
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Status = ( int )statusCode,
            Title = title,
            Instance = context.Request.Path
        };

        return context.Response.WriteAsJsonAsync( problemDetails );
    }
}