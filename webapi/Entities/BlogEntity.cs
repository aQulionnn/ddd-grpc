using System.ComponentModel.DataAnnotations;
using webapi.DomainEvents;
using webapi.Primitives;
using webapi.ValueObjects;

namespace webapi.Entities;

public sealed class BlogEntity : AggregateRoot
{
    private readonly List<PostEntity> _posts = new(); 
    
    private BlogEntity(Guid id, string name) 
        : base(id)
    {
        Name = name; 
    }
    
    public string Name { get; private set; }
    public IReadOnlyCollection<PostEntity> Posts => _posts;

    public static BlogEntity Create(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("BlogEntity name is required.");

        var blog = new BlogEntity(id, name);
        blog.RaiseDomainEvent(new BlogCreatedDomainEvent(id, name));
        
        return blog;
    }

    public PostEntity AddPost(Guid postId, string title, string content)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException("PostEntity title is required.");
        
        if (string.IsNullOrWhiteSpace(content))
            throw new ValidationException("PostEntity content is required.");
        
        var post = new PostEntity(postId, this, Title.Create(title), Content.Create(content));
        _posts.Add(post);
        return post;
    }
}