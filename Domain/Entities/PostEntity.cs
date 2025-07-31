using Domain.Policies;
using Domain.ValueObjects;
using SharedKernel.Primitives;

namespace Domain.Entities;

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
    
    public void SetPublishDate(DateTime date)
    {
        PublishedAt = date;
    }
}