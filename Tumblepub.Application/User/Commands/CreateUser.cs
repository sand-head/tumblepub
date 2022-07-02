﻿using Tumblepub.Application.Interfaces;

namespace Tumblepub.Application.User.Commands;

public record CreateUserCommand(string Email, string Username, string Password) : ICommand<Models.User>;

internal class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Models.User>
{
    private readonly IRepository<Models.User, Guid> _userRepository;
    private readonly IRepository<Models.Blog, Guid> _blogRepository;

    public CreateUserCommandHandler(IRepository<Models.User, Guid> userRepository, IRepository<Models.Blog, Guid> blogRepository)
    {
        _userRepository = userRepository;
        _blogRepository = blogRepository;
    }
    
    public async Task<Models.User> Handle(CreateUserCommand command, CancellationToken token = default)
    {
        var (email, name, password) = command;
        
        // todo: probably wrap these in a transaction
        var user = new Models.User(email, password);
        await _userRepository.CreateAsync(user, token);
        await _userRepository.SaveChangesAsync(token); // not sure how much I like this...
        
        // all users need at least one blog
        var blog = new Models.Blog(user.Id, name);
        await _blogRepository.CreateAsync(blog, token);
        await _blogRepository.SaveChangesAsync(token);

        return user;
    }
}