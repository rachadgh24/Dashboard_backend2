using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test2.DataLayer.Entities;

namespace Test2.DataLayer.Interfaces
{
    public interface IPostRepository
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
