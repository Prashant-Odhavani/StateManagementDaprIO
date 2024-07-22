using Dapr.Client;
using StateManagementDaprIO.Common.Dtos;
using StateManagementDaprIO.Common.Models;
using StateManagementDaprIO.Services.Interface;

namespace StateManagementDaprIO.Services
{
    public class BlogService : IBlogService
    {
        private static string STORE_NAME = "blogPostStatestore";
        private readonly DaprClient _daprClient;

        public BlogService(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task<string> CreateBlogPost(BlogPostDto blogPostDto)
        {
            var blogPost = new BlogPost
            {
                Id = Guid.NewGuid().ToString(),
                Title = blogPostDto.Title,
                Content = blogPostDto.Content,
                Author = blogPostDto.Author,
                Category = blogPostDto.Category,
                CreatedAt = DateTime.UtcNow
            };

            await _daprClient.SaveStateAsync<BlogPost>(STORE_NAME, blogPost.Id, blogPost);
            return blogPost.Id;
        }

        public async Task<bool> DeleteBlogPost(string id)
        {
            await _daprClient.DeleteStateAsync(STORE_NAME, id);
            return true;
        }

        public async Task<BlogPost?> GetBlogPostById(string id)
        {
            var blogPost = await _daprClient.GetStateAsync<BlogPost>(STORE_NAME, id);
            return blogPost;
        }

        public async Task<List<BlogPost>> GetBlogPostsByCategory(string category)
        {
            var query = "{" +
                "\"filter\": {" +
                    "\"EQ\": { \"category\": \"" + category + "\" }" +
                "}}";

            var queryResponse = await _daprClient.QueryStateAsync<BlogPost>(STORE_NAME, query);

            var blogPostList = queryResponse.Results.Select(q => q.Data).OrderByDescending(q => q.CreatedAt).ToList();

            return blogPostList;
        }

        public async Task<bool> UpdateBlogPost(string id, BlogPostDto blogPostDto)
        {
            var currentBlogPost = await _daprClient.GetStateAsync<BlogPost>(STORE_NAME, id);
            if (currentBlogPost is not null)
            {
                currentBlogPost.Title = blogPostDto.Title;
                currentBlogPost.Content = blogPostDto.Content;
                currentBlogPost.Author = blogPostDto.Author;
                currentBlogPost.Category = blogPostDto.Category;
                currentBlogPost.UpdatedAt = DateTime.UtcNow;
                await _daprClient.SaveStateAsync<BlogPost>(STORE_NAME, currentBlogPost.Id, currentBlogPost);
                return true;
            }
            return false;
        }
    }
}
