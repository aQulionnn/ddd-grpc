using webapi.Entities;

namespace webapi.Abstractions.Policies;

public interface IPostSchedulerPolicy
{
    void Schedule(PostEntity postEntity);
}