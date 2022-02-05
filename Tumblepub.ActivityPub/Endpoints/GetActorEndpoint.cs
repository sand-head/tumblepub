﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Tumblepub.ActivityPub.Endpoints;

public sealed class GetActorEndpoint : Endpoint
{
    private readonly IActivityPubService _activityPubService;
    private readonly ILogger<GetActorEndpoint> _logger;

    public GetActorEndpoint(IActivityPubService activityPubService, ILogger<GetActorEndpoint> logger)
    {
        _activityPubService = activityPubService ?? throw new ArgumentNullException(nameof(activityPubService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<IActionResult> InvokeAsync(HttpContext context, RouteData? routeData, CancellationToken token = default)
    {
        var userId = Guid.Parse(routeData!.Values["userId"]!.ToString()!);
        var user = await _activityPubService.GetActor(userId, token);

        if (user == null)
        {
            _logger.LogInformation("Actor {Id} not found.", userId);
            return NotFound();
        }

        _logger.LogInformation("Actor {Id} found.", userId);
        return Ok(user);
    }
}