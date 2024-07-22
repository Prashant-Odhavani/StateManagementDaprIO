using StateManagementDaprIO.Common.Dtos;
using StateManagementDaprIO.Common.Models;

namespace StateManagementDaprIO.Services.Interface
{
    public interface IBlogService
    {
        Task<string> CreateBlogPost(BlogPostDto blogPostDto);
        Task<bool> DeleteBlogPost(string id);
        Task<BlogPost?> GetBlogPostById(string id);
        Task<List<BlogPost>> GetBlogPostsByCategory(string category);
        Task<bool> UpdateBlogPost(string id, BlogPostDto blogPostDto);
    }
}
