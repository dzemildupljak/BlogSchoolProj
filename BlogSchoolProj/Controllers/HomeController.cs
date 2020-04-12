using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogSchoolProj.Models;
using BlogSchoolProj.Data;
using BlogSchoolProj.Data.FileManager;
using BlogSchoolProj.Models.Comment;
using BlogSchoolProj.ViewModels;

namespace BlogSchoolProj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IRepository _repo;
        private IFileManager _fileManage;

        public HomeController(ILogger<HomeController> logger, IRepository repo, IFileManager fileManager)
        {
            _logger = logger;
            _repo = repo;
            _fileManage = fileManager;
        }

        public IActionResult Index(int pageNumber, string category)
        {
            if (pageNumber < 1)
            {
                return RedirectToAction("Index", new { pageNumber = 1, category } );
            }
            var postsVm = _repo.GetAllPosts(pageNumber, category);
            //boolean ? true : false   1=1 ? run : ignore
            return View(postsVm);
        }

        public IActionResult Post(int id)
        {
            var post = _repo.GetPost(id);
            return View(post);
        }

        [HttpGet("Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManage.ImageStream(image), $"image/{mime}");
        }
        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id= vm.PostId });
            }
            var post = _repo.GetPost(vm.PostId);
            if (vm.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();

                post.MainComments.Add(new MainComment
                {
                    Message = vm.Massage,
                    CreatedComment = DateTime.Now
                });

                _repo.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    Message = vm.Massage,
                    CreatedComment = DateTime.Now
                };
            }

            await _repo.SaveChangesAsync();
            return RedirectToAction("Post", new { id = vm.PostId });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
