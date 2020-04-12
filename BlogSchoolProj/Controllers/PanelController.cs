using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogSchoolProj.Data;
using BlogSchoolProj.Data.FileManager;
using BlogSchoolProj.Models;
using BlogSchoolProj.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSchoolProj.Controllers
{
    [Authorize(Roles = "admin")]
    public class PanelController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public PanelController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
        }
        public IActionResult Index()
        {
            var allposts = _repo.GetAllPosts();
            return View(allposts);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View(new PostViewModel());
            }
            else
            {
                var post = _repo.GetPost((int)id);
                return View(new PostViewModel 
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    CurrentImage = post.Image,
                    Category = post.Category,
                    Tags = post.Tags,
                    Description = post.Description
                });
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            var post = new Post
            {
                Id = vm.Id,
                Title = vm.Title,
                Body = vm.Body,
                Category = vm.Category,
                Tags = vm.Tags,
                Description = vm.Description
            };
            if (vm.Image == null)
            {
                post.Image = vm.CurrentImage;
            }
            else
            {
                post.Image = await _fileManager.SaveImage(vm.Image);
            }

            if (post.Id > 0)
            {
                _repo.UpdatePost(post);
            }
            else
            {
                _repo.AddPost(post);
            }
            if (await _repo.SaveChangesAsync())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(post);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            _repo.DeletPost(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}