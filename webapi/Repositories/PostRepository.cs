using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using webapi.Mappers;

namespace webapi.Repositories;

public class PostRepository(AppDbContext context)
{
    private readonly AppDbContext _context =  context;

    public async Task<PostEntity> AddAsync(Post post)
    {
        var blog = await _context.Blogs
            .Include(blog => blog.Posts)
            .FirstOrDefaultAsync(blog => blog.Id == post.BlogId);
        
        if  (blog is null) 
            throw new Exception("Blog not found");
        
        var newPost = BlogMapper.ToEntity(blog).AddPost(Guid.NewGuid(), post.Title, post.Content);

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        
        return newPost;
    }
    
    public async Task<IEnumerable<PostEntity>> GetByBlogIdAsync(Guid blogId)
    {
        var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == blogId);
        if  (blog is null) 
            throw new Exception("Blog not found");
        
        var posts = BlogMapper.ToEntity(blog).Posts.ToList();
        return posts;
    }
}