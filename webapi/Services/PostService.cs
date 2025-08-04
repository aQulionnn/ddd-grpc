using Contracts.Protos;
using Domain.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using webapi.Repositories;

namespace webapi.Services;

public class PostService(PostRepository postRepository)
    : Contracts.Protos.PostService.PostServiceBase
{
    private readonly PostRepository _postRepository = postRepository;

    public override async Task<PostResponse> AddPost(AddPostRequest request, ServerCallContext context)
    {
        var post = new Post
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            PublishedAt = DateTime.Now,
            BlogId = Guid.Parse(request.BlogId)
        };
            
        var postEntity = await _postRepository.AddAsync(post);

        return new PostResponse
        {
            Id = postEntity.Id.ToString(),
            Title = postEntity.Title.Value,
            Content = postEntity.Content.Value,
            PublishedAt = Timestamp.FromDateTime(postEntity.PublishedAt),
            BlogId = postEntity.BlogEntity.Id.ToString(),
        };
    }

    public override async Task<PostsResponse> GetPostsByBlogId(GetPostsByBlogIdRequest request, ServerCallContext context)
    {
        var response = new PostsResponse();
        
        var posts = await _postRepository.GetByBlogIdAsync(Guid.Parse(request.BlogId));
        
        response.Posts.AddRange(posts.Select(post => new PostResponse
        {
            Id = post.Id.ToString(),
            Title = post.Title.Value,
            Content = post.Content.Value,
            PublishedAt = Timestamp.FromDateTime(post.PublishedAt),
            BlogId = post.BlogEntity.Id.ToString(),
        }));
        
        return response;
    }
}