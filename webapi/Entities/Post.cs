using webapi.Primitives;

namespace webapi.Entities;

public sealed class Post : Entity 
{
    internal Post(Guid id, Blog blog, string title, string content) 
        : base(id)
    {
        Blog = blog;
        Title = title;
        Content = content;
        PublishedAt = DateTime.UtcNow;
    }
    
    public Blog Blog { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public DateTime PublishedAt { get; private set; }
}