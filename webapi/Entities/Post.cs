using webapi.Primitives;
using webapi.ValueObjects;

namespace webapi.Entities;

public sealed class Post : Entity 
{
    internal Post(Guid id, Blog blog, Title title, Content content) 
        : base(id)
    {
        Blog = blog;
        Title = title;
        Content = content;
        PublishedAt = DateTime.Now;
    }
    
    public Blog Blog { get; private set; }
    public Title Title { get; private set; }
    public Content Content { get; private set; }
    public DateTime PublishedAt { get; private set; }
}