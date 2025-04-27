using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace YellowHouseStudio.LifeOrbit.Tests.Common.Assertions;

public static class HttpResponseAssertions
{
    public static async Task ShouldBeOk<T>(this HttpResponseMessage response, Action<T>? assertions = null)
    {
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<T>();
        result.Should().NotBeNull();
        assertions?.Invoke(result!);
    }

    public static async Task ShouldBeBadRequest(this HttpResponseMessage response, string? expectedError = null)
    {
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        if (expectedError != null)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            problemDetails.Should().NotBeNull();
            problemDetails!.Title.Should().Be("One or more validation errors occurred.");
            problemDetails.Errors.Should().ContainValue(new[] { expectedError });
        }
    }

    public static async Task ShouldBeNotFound(this HttpResponseMessage response, string? expectedMessage = null)
    {
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        if (expectedMessage != null)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            problemDetails.Should().NotBeNull();
            problemDetails!.Detail.Should().Be(expectedMessage);
        }
    }
}