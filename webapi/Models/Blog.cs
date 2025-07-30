namespace webapi.Models;

public class Blog
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public IEnumerable<Post> Posts { get; set; }
}