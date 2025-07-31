using Domain.Entities;

namespace Domain.Policies;

public interface IPostSchedulerPolicy
{
    void Schedule(PostEntity postEntity);
}