using Microsoft.AspNetCore.Mvc;
using StateManagementDaprIO.Common.Dtos;
using StateManagementDaprIO.Services.Interface;

namespace StateManagementDaprIO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogPostsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] BlogPostDto blogPostDto)
        {
            if (blogPostDto == null)
            {
                return BadRequest();
            }

            var blogPostId = await _blogService.CreateBlogPost(blogPostDto);
            return CreatedAtAction(nameof(GetBlogPostById), new { id = blogPostId }, new { id = blogPostId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogPostById(string id)
        {
            var blogPost = await _blogService.GetBlogPostById(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            return Ok(blogPost);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetBlogPostsByCategory(string category)
        {
            var blogPosts = await _blogService.GetBlogPostsByCategory(category);

            if (blogPosts == null || !blogPosts.Any())
            {
                return NotFound();
            }

            return Ok(blogPosts);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogPost(string id, [FromBody] BlogPostDto blogPostDto)
        {
            if (blogPostDto == null)
            {
                return BadRequest();
            }

            var isUpdated = await _blogService.UpdateBlogPost(id, blogPostDto);

            if (!isUpdated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(string id)
        {
            var isDeleted = await _blogService.DeleteBlogPost(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
