using Domain.Entities;
using Domain.Policies;

namespace webapi.Policies;

public class TimeBasedPostScheduler(TimeSpan delay)
    : IPostSchedulerPolicy
{
    private readonly TimeSpan _delay = delay;
    
    public void Schedule(PostEntity postEntity)
    {
        postEntity.SetPublishDate(DateTime.UtcNow.Add(_delay));
    }
}