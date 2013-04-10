using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YOrganize.Showcase.Web.Infrastructure;
using Yorganize.Business.Exceptions;
using Yorganize.Business.Providers.Storage;
using Yorganize.Business.Repository;
using Yorganize.Showcase.Domain.Models;
using AutoMapper;
using Yorganize.Showcase.Web.Models;
using System.Transactions;

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

        [HttpPost]
        //[Authorize] TODO: uncomment this in production
        public ActionResult SaveVideo(VideoModel model, HttpPostedFileBase sourceMP4, HttpPostedFileBase sourceOGG, HttpPostedFileBase sourceWEBM)
        {
            bool isNew = model.ID == Guid.Empty;

            Video video = isNew ? new Video() : _videoRepository.FindByID(model.ID);

            if (video == null)
                throw new BusinessException("Video not found, it might have been removed.");

            Mapper.Map(model, video);

            if (isNew)
                video.ID = Guid.NewGuid();

            using (var ts = new TransactionScope())
            {
                try
                {
                    if (sourceMP4 != null)
                        video.SourceMP4Url = UploadVideoSource(video.ID, sourceMP4).AbsoluteUri;

                    if (sourceOGG != null)
                        video.SourceOGGUrl = UploadVideoSource(video.ID, sourceOGG).AbsoluteUri;

                    if (sourceWEBM != null)
                        video.SourceWEBMUrl = UploadVideoSource(video.ID, sourceWEBM).AbsoluteUri;

                }
                catch (Exception ex)
                {
                    throw new BusinessException("Failed to upload video sources.", ex);
                }

                try
                {
                    // update the database

                    if (isNew)
                        _videoRepository.Insert(video);
                    else
                        _videoRepository.Update(video);

                    ts.Complete();
                }
                catch (Exception ex)
                {
                    // TODO: remove the sources from storage if new
                    throw new BusinessException("Failed to save video information.", ex);
                }
            }

            Mapper.Map(video, model);

            return new JsonNetResult(model);
        }

        private Uri UploadVideoSource(Guid videoID, HttpPostedFileBase file)
        {
            if (file == null)
                return null;

            string path = string.Format("showcase/video/{0}{1}", videoID, System.IO.Path.GetExtension(file.FileName));
            StorageProviderManager.Provider.UploadFile(file.InputStream, path);

            return StorageProviderManager.Provider.GetFileUri(path);
        }

    }
}