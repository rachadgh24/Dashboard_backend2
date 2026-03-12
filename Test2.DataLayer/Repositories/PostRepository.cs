using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test2.DataLayer.DbContexts;
using Test2.DataLayer.Entities;
using Test2.DataLayer.Interfaces;
namespace Test2.DataLayer.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DotNetTrainingCoreContext _context;

public PostRepository(DotNetTrainingCoreContext context)
{
    _context = context;
}
        public IQueryable<Post> GetAll(){
            return _context.Posts;
        }
        public IQueryable<Post> GetById(int id){
            return _context.Posts.Where(p => p.Id == id);
        }
        public  async Task<Post> CreatePostAsync(Post post){
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }
        public async Task<Post> Update(Post post){
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }
        public async Task<Post> Delete(int id){
            var post = _context.Posts.Find(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return post;
        }
         public async Task<List<Post>> PaginatePosts(int page){
        return await _context.Posts
                .OrderBy(p => p.Id)
                .Skip((page - 1) * 4)
                .Take(4)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync() => await _context.Posts.CountAsync();
    }
}
