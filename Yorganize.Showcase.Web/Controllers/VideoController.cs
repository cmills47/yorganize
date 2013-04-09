using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YOrganize.Showcase.Web.Infrastructure;
using Yorganize.Business.Repository;
using Yorganize.Showcase.Domain.Models;
using AutoMapper;
using Yorganize.Showcase.Web.Models;

namespace Yorganize.Showcase.Web.Controllers
{
    public class VideoController : Controller
    {

        private readonly IKeyedRepository<Guid, Video> _videoRepository;
        private readonly IKeyedRepository<int, VideoCategory> _categoryRepository;

        public VideoController(IKeyedRepository<Guid, Video> videoRepository, IKeyedRepository<int, VideoCategory> categoryRepository)
        {
            _videoRepository = videoRepository;
            _categoryRepository = categoryRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetVideoCategories()
        {
            var categories = _categoryRepository.All().ToList();
            var models = Mapper.Map<List<VideoCategory>, List<VideoCategoryModel>>(categories);

            return new JsonNetResult(models);
        }

        public ActionResult GetVideoList(string category)
        {
            var videos = _videoRepository.All().Where(video => video.Category.Name == category).ToList();
            var videoModels = Mapper.Map<List<Video>, List<VideoModel>>(videos);

            return new JsonNetResult(new VideoListModel()
                {
                    Category = category,
                    Videos = videoModels
                });
        }
    }
}