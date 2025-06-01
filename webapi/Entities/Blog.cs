using System.ComponentModel.DataAnnotations;
using webapi.Primitives;

namespace webapi.Entities;

public sealed class Blog : Entity
{
    private readonly List<Post> _posts = new(); 
    
    private Blog(Guid id, string name) 
        : base(id)
    {
        Name = name; 
    }
    
    public string Name { get; private set; }
    public IReadOnlyCollection<Post> Posts => _posts;

    public static Blog Create(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Blog name is required.");
        
        return new Blog(id, name);;
    }

    public Post AddPost(Guid postId, string title, string content)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException("Post title is required.");
        
        if (string.IsNullOrWhiteSpace(content))
            throw new ValidationException("Post content is required.");
        
        var post = new Post(postId, this, title, content);
        _posts.Add(post);
        return post;
    }
}