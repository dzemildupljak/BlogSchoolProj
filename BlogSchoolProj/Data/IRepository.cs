using BlogSchoolProj.Models;
using BlogSchoolProj.Models.Comment;
using BlogSchoolProj.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSchoolProj.Data
{
    public interface IRepository
    {
        Post GetPost(int id);
        List<Post> GetAllPosts();
        IndexViewModel GetAllPosts(int pageNumber, string category);
        void AddPost(Post post);
        void UpdatePost(Post post);
        void DeletPost(int id);

        Task<bool> SaveChangesAsync();
    }
}
