using System;
using System.Linq;
using System.Web.Mvc;
using Yorganize.Business.Exceptions;
using Yorganize.Business.Repository;
using Yorganize.Showcase.Domain.Models;
using System.Collections.Generic;
using Yorganize.Showcase.Web.Models;
using AutoMapper;

namespace Yorganize.Showcase.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly IKeyedRepository<Guid, BlogPost> _blogPostRepository;

        public BlogController(IKeyedRepository<Guid, BlogPost> blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public ActionResult Index()
        {
            _blogPostRepository.BeginTransaction();

            var q = from post in _blogPostRepository.All()
                    select post;

            var posts = q.ToList();

            _blogPostRepository.CommitTransaction();

            var model = Mapper.Map<List<BlogPost>, List<BlogPostItemModel>>(posts);

            return View(model);
        }

        public ActionResult Post(string id, bool edit = false)
        {
            BlogPostModel model;

            if (string.IsNullOrEmpty(id)) // new post request
            {
                if (!Request.IsAuthenticated)
                    throw new BusinessException("You are not authorized to create new posts.");
                // create new post
                model = new BlogPostModel()
                            {
                                Title = "please fill in the title",
                                Header = "please fill in the header information",
                                Content = "<br/>&nbsp;please fill in the post content",
                                Author = User.Identity.Name
                            };

                return View("Edit", model); // new post
            }

            // a post was specified
            if (edit && !Request.IsAuthenticated)
                throw new BusinessException("You are not authorized to edit this post.");

            // read the post from database
            var post = _blogPostRepository.All().SingleOrDefault(p => p.Slug == id);

            if (post == null)
                throw new BusinessException(string.Format("Could not find post: {0}", id));

            model = Mapper.Map<BlogPost, BlogPostModel>(post);

            if (edit)
                return View("Edit", model); // open the post for editing

            // open the post for viewing
            return View(model);
        }

    }
}
