using BlogSchoolProj.Models;
using BlogSchoolProj.Models.Comment;
using BlogSchoolProj.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSchoolProj.Data
{
    public class Repository : IRepository
    {
        private ApplicationDbContext _db;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddPost(Post post)
        {
            _db.Add(post);
        }

        public void DeletPost(int id)
        {
            _db.Posts.Remove(GetPost(id));
        }

        public List<Post> GetAllPosts()
        {
            var posts = _db.Posts.ToList();
            return posts;
        }
        public IndexViewModel GetAllPosts(int pageNumber, string category)
        {
            //var posts = _db.Posts.Where(p => p.Category.ToLower().Equals(category.ToLower())).ToList();
            //Func<Post, bool> InCategory = (post) => { return post.Category.ToLower().Equals(category.ToLower()); }; 
            
            int pageSize = 5;
            int skipAmount = pageSize * (pageNumber - 1);
            int capacity = skipAmount + pageSize;
            var query = _db.Posts.Skip(skipAmount).Take(pageSize);
            int postCount = _db.Posts.Count();

            if (!String.IsNullOrEmpty(category))
            {
                var queryVm = _db.Posts.Where(p => p.Category.ToLower().Equals(category.ToLower())).Skip(skipAmount).Take(pageSize);
                int postCountVm = query.Count();
                var postsVm = new IndexViewModel
                {
                    PageNumber = pageNumber,
                    PageCount = (int)Math.Ceiling((double)postCountVm / pageSize),
                    NextPage = postCountVm > capacity,
                    Category = category,
                    Posts = queryVm.ToList()
                };

                return postsVm;
            }
            var posts = new IndexViewModel
            {
                PageNumber = pageNumber,
                PageCount = (int) Math.Ceiling((double)postCount / pageSize),
                NextPage = postCount > capacity,
                Posts = query.ToList()
            };
            return posts;

        }

        public Post GetPost(int id)
        {
            var post = _db.Posts
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.SubComments)
                .FirstOrDefault(p => p.Id == id);
            return post;
        }

        public void UpdatePost(Post post)
        {
            _db.Posts.Update(post);
        }

        public async Task<bool> SaveChangesAsync()
        {
            if (await _db.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public void AddSubComment(SubComment subComment)
        {
            _db.SubComments.Add(subComment);
        }
    }
}
