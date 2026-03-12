using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test2.Application.Interfaces;
using Test2.DataLayer.Entities;
using Test2.Helpers;

namespace Test2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTotalCount(CancellationToken cancellationToken = default)
        {
            var count = await _postService.GetTotalCountAsync();
            return Ok(count);
        }

        [HttpGet("paginate")]
        public async Task<ActionResult<IEnumerable<Post>>> Paginate([FromQuery] int page = 1, CancellationToken cancellationToken = default)
        {
            if (page < 1)
                return BadRequest("Page must be at least 1.");
            var posts = await _postService.PaginatePosts(page);
            return Ok(posts);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetAll(CancellationToken cancellationToken = default)
        {
            var posts = await _postService.GetAll().ToListAsync(cancellationToken);
            return Ok(posts);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Post>> GetById(int id, CancellationToken cancellationToken = default)
        {
            var post = await _postService.GetById(id).FirstOrDefaultAsync(cancellationToken);
            if (post == null)
                return NotFound();
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Post post, CancellationToken cancellationToken = default)
        {
            var addedPost = await _postService.CreatePostAsync(post);
            var role = UserClaimsHelper.GetRole(User);
            var name = UserClaimsHelper.GetDisplayName(User);
            var message = $"The {role} {name} just posted!";
            return Ok(new { post = addedPost, message });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Post>> Update(int id, [FromBody] Post post, CancellationToken cancellationToken = default)
        {
            var existing = await _postService.GetById(id).FirstOrDefaultAsync(cancellationToken);
            if (existing == null)
                return NotFound();

            post.Id = id;
            var updated = await _postService.Update(post);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var existing = await _postService.GetById(id).FirstOrDefaultAsync(cancellationToken);
            if (existing == null)
                return NotFound();

            await _postService.Delete(id);
            return NoContent();
        }
    }
}
