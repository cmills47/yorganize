using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YOrganize.Showcase.Web.Infrastructure;
using Yorganize.Business.Exceptions;
using Yorganize.Business.Providers.Storage;
using Yorganize.Business.Repository;
using Yorganize.Showcase.Domain.Models;
using System.Collections.Generic;
using Yorganize.Showcase.Web.Models;
using AutoMapper;
using System.Transactions;

namespace Yorganize.Showcase.Web.Controllers
{
    public class BlogController : BaseController
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
                    orderby post.Created descending
                    select post;

            var posts = q.ToList();

            _blogPostRepository.CommitTransaction();

            var model = Mapper.Map<List<BlogPost>, List<BlogPostItemModel>>(posts);

            return View(model);
        }

        /// <summary>
        /// Create or edit a post
        /// </summary>
        /// <param name="id">The 'Slug' of the post</param>
        /// <param name="edit">If true, open in edit mode</param>
        /// <returns></returns>
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

        [HttpPost]
        [Authorize]
        public ActionResult SavePost(BlogPostModel model)
        {
            var post = new BlogPost();

            Mapper.Map(model, post);

            if (string.IsNullOrEmpty(post.Author))
                post.Author = User.Identity.Name;
            try
            {
                _blogPostRepository.Save(post);
            }
            catch (Exception ex)
            {
                throw new BusinessException("There is already a post with the same title.", ex);
            }

            Alert("The post has been saved.", "alert-success");
            return RedirectToAction("Post", new { id = post.Slug, edit = true });
        }

        public ActionResult RemovePost(Guid id)
        {
            if (!Request.IsAuthenticated)
                throw new BusinessException("You are not authorized to remove this post.");

            using (TransactionScope ts = new TransactionScope())
            {
                _blogPostRepository.BeginTransaction();
                var post = _blogPostRepository.FindByID(id);
                _blogPostRepository.CommitTransaction();

                if (post == null)
                    throw new BusinessException("Failed to retrieve post or post has already been removed.");

                _blogPostRepository.Delete(post);

                ts.Complete();
            }

            Alert("The post has been removed", "alert-success");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadImage(Guid postID, IEnumerable<HttpPostedFileBase> files)
        {
            var filesList = files as IList<HttpPostedFileBase> ?? files.ToList();

            if (filesList.Count() != 1)
                throw new BusinessException("Oops, something has gone wrong with your file!");

            var file = filesList.First();

            //TODO: check file content type
            string path = string.Format("showcase/blog/posts/images/{0}{1}", postID, System.IO.Path.GetExtension(file.FileName));
            StorageProviderManager.Provider.UploadFile(file.InputStream, path);
            var Uri = StorageProviderManager.Provider.GetFileUri(path);

            return new JsonNetResult(Uri.AbsoluteUri);
        }

    }
}
