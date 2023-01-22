﻿using Tumblepub.Application.Interfaces;

namespace Tumblepub.Application.Blog.Queries;

public record GetBlogByIdQuery(Guid Id) : IQuery<Aggregates.Blog?>;

internal class GetBlogByIdQueryHandler : IQueryHandler<GetBlogByIdQuery, Aggregates.Blog?>
{
    private readonly IReadOnlyRepository<Aggregates.Blog, Guid> _blogRepository;

    public GetBlogByIdQueryHandler(IReadOnlyRepository<Aggregates.Blog, Guid> blogRepository)
    {
        _blogRepository = blogRepository;
    }
    
    public async Task<Aggregates.Blog?> Handle(GetBlogByIdQuery query, CancellationToken token = default)
    {
        return await _blogRepository.GetByIdAsync(query.Id, token);
    }
}