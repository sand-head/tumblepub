﻿using Marten;
using Microsoft.Extensions.Logging;
using Tumblepub.Application.Events;
using Tumblepub.Application.Interfaces;
using Tumblepub.Application.Models;

namespace Tumblepub.Infrastructure.Repositories;

internal class PostRepository : IPostRepository
{
    private readonly ILogger<PostRepository> _logger;
    private readonly IDocumentSession _session;

    public PostRepository(ILogger<PostRepository> logger, IDocumentSession session)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }

    public Task<Post> CreateExternalPost(Guid blogId, Uri externalPostUrl, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Post> CreateMarkdownPost(Guid blogId, string content, IEnumerable<string>? tags, CancellationToken token = default)
    {
        var markdownContent = new PostContent.Markdown(content);
        var postCreated = new PostCreated(Guid.NewGuid(), blogId, markdownContent, DateTimeOffset.UtcNow);

        _session.Events.StartStream<Post>(postCreated.PostId, postCreated);
        await _session.SaveChangesAsync(token);
        _logger.LogInformation("Created new post {PostId} on blog {BlogId}", postCreated.PostId, postCreated.BlogId);

        var post = await _session.LoadAsync<Post>(postCreated.PostId, token);
        return post!;
    }

    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _session.LoadAsync<Post>(id, token);
    }
}
