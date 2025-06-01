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
}