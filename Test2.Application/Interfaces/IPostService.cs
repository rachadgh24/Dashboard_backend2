using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test2.DataLayer.Entities;

namespace Test2.Application.Interfaces
{
    public interface IPostService
    {
        IQueryable<Post> GetAll();
        IQueryable<Post> GetById(int id);
        Task<Post> CreatePostAsync(Post post);
        Task<Post> Update(Post post);
        Task<Post> Delete(int id);
        Task<List<Post>> PaginatePosts(int page);
        Task<int> GetTotalCountAsync();
    }
}
