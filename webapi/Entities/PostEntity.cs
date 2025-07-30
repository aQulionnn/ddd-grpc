using webapi.Abstractions.Policies;
using webapi.Primitives;
using webapi.ValueObjects;

namespace webapi.Entities;

public sealed class PostEntity : Entity 
{
    internal PostEntity(Guid id, BlogEntity blogEntity, Title title, Content content) 
        : base(id)
    {
        BlogEntity = blogEntity;
        Title = title;
        Content = content;
        PublishedAt = DateTime.Now;
    }
    
    public BlogEntity BlogEntity { get; private set; }
    public Title Title { get; private set; }
    public Content Content { get; private set; }
    public DateTime PublishedAt { get; private set; }

    public void Schedule(IPostSchedulerPolicy policy)
    {
        policy.Schedule(this);
    }
    
    internal void SetPublishDate(DateTime date)
    {
        PublishedAt = date;
    }
}