using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test2.Application.Interfaces;
using Test2.DataLayer.Entities;
using Test2.DataLayer.Interfaces;

namespace Test2.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public IQueryable<Post> GetAll() => _postRepository.GetAll();

        public IQueryable<Post> GetById(int id) => _postRepository.GetById(id);

        public async Task<Post> CreatePostAsync(Post post) => await _postRepository.CreatePostAsync(post);

        public async Task<Post> Update(Post post) => await _postRepository.Update(post);

        public async Task<Post> Delete(int id) => await _postRepository.Delete(id);

        public async Task<List<Post>> PaginatePosts(int page) => await _postRepository.PaginatePosts(page);

        public async Task<int> GetTotalCountAsync() => await _postRepository.GetTotalCountAsync();
    }
}
