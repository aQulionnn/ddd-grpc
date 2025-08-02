using Domain.Entities;
using Domain.Models;

namespace webapi.Mappers;

public static class BlogMapper
{
    public static BlogEntity ToEntity(Blog blog)
    {
        var entity = BlogEntity.Create(blog.Id, blog.Name);
        
        foreach (var post in blog.Posts)
            entity.AddPost(post.Id,post.Title, post.Content);
        
        return entity;
    } 
}