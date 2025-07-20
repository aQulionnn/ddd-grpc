using webapi.Abstractions.Policies;
using webapi.Entities;

namespace webapi.Policies;

public class TimeBasedPostScheduler(TimeSpan delay)
    : IPostSchedulerPolicy
{
    private readonly TimeSpan _delay = delay;
    
    public void Schedule(Post post)
    {
        post.SetPublishDate(DateTime.UtcNow.Add(_delay));
    }
}