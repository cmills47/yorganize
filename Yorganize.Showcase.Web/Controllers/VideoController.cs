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

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetVideoCategories()
        {
            var categories = _categoryRepository.All().ToList();
            var models = Mapper.Map<List<VideoCategory>, List<VideoCategoryModel>>(categories);

            return new JsonNetResult(models);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetVideoList(string category)
        {
            var videos = _videoRepository.All().
                Where(video => video.Category.Name == category)
                .OrderBy(video => video.Order)
                .ToList();

            var videoModels = Mapper.Map<List<Video>, List<VideoModel>>(videos);

            return new JsonNetResult(new VideoListModel()
                {
                    Category = category,
                    Videos = videoModels
                });
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveVideo(VideoModel model, HttpPostedFileBase sourceMP4, HttpPostedFileBase sourceOGG, HttpPostedFileBase sourceWEBM)
        {
            bool isNew = model.ID == Guid.Empty;

            Video video = isNew ? new Video() : _videoRepository.FindByID(model.ID);

            if (video == null)
                throw new BusinessException("Video not found, it might have been removed.");

            int oldOrder = video.Order;
            int newOrder = model.Order;

            Mapper.Map(model, video);

            if (isNew)
                video.ID = Guid.NewGuid();

            using (var ts = new TransactionScope())
            {
                if (isNew) // create video if new
                    try
                    {
                        _videoRepository.Insert(video);
                    }
                    catch (Exception ex)
                    {
                        throw new BusinessException("Failded to create video. Another video with the same title might already exist in this category.", ex);
                    }

                // upload / update the video sources
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
                    //todo: remove uploaded sources
                    throw new BusinessException("Failed to upload video sources.", ex);
                }

                // update the database
                try
                {
                    // update order
                    if (isNew)
                    {
                        // get max order for the category
                        var maxOrder = _videoRepository.All().Count(v => v.Category.ID == video.Category.ID);

                        // set order and insert
                        video.Order = maxOrder + 1;
                        _videoRepository.Update(video);

                    }
                    else
                    {
                        // compare current order with new order
                        dynamic unordered = null;

                        // if order has changed, update order for items in between
                        _videoRepository.BeginTransaction();
                        if (newOrder < oldOrder) // order decreased
                        {
                            unordered = _videoRepository.FilterBy(
                                v =>
                                    v.ID != video.ID &&
                                    v.Category.ID == video.Category.ID &&
                                    v.Order >= newOrder && v.Order < oldOrder
                                );

                            if (unordered != null)
                                foreach (var uitem in unordered)
                                    uitem.Order++;
                        }
                        else if (newOrder > oldOrder) // order increased
                        {
                            unordered = _videoRepository.FilterBy(
                                v =>
                                    v.ID != video.ID &&
                                    v.Category.ID == video.Category.ID &&
                                    v.Order > oldOrder && v.Order <= newOrder
                                );

                            if (unordered != null)
                                foreach (var uitem in unordered)
                                    uitem.Order--;
                        }
                        _videoRepository.CommitTransaction();

                        if (unordered != null)
                            _videoRepository.Update(unordered);

                        _videoRepository.Update(video);
                    }

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

        [HttpPut]
        public ActionResult UpdateVideo(VideoModel model)
        {
            return SaveVideo(model, null, null, null);
        }

        private Uri UploadVideoSource(Guid videoID, HttpPostedFileBase file)
        {
            if (file == null)
                return null;

            string path = string.Format("showcase/video/{0}{1}", videoID, System.IO.Path.GetExtension(file.FileName));
            StorageProviderManager.Provider.UploadFile(file.InputStream, path, file.ContentType);

            return StorageProviderManager.Provider.GetFileUri(path);
        }

        [HttpDelete]
        [Authorize]
        public ActionResult RemoveVideo(Guid id)
        {
            var video = _videoRepository.FindByID(id);

            if (video == null)
                throw new BusinessException("Video not found, it might have been already removed.");

            using (var ts = new TransactionScope())
            {
                try
                {
                    // delete the video sources
                    if (!string.IsNullOrEmpty(video.SourceMP4Url))
                        StorageProviderManager.Provider.DeleteFile(new Uri(video.SourceMP4Url));

                    if (!string.IsNullOrEmpty(video.SourceOGGUrl))
                        StorageProviderManager.Provider.DeleteFile(new Uri(video.SourceOGGUrl));

                    if (!string.IsNullOrEmpty(video.SourceWEBMUrl))
                        StorageProviderManager.Provider.DeleteFile(new Uri(video.SourceWEBMUrl));

                }
                catch (Exception ex)
                {
                    //TODO: log exception
                    throw new BusinessException("Failed to remove video sources.");
                }

                try
                {
                    // update siblings order
                    _videoRepository.BeginTransaction();
                    var unordered = _videoRepository.FilterBy(v =>
                          v.Category.ID == video.Category.ID && v.Order > video.Order);

                    if (unordered != null)
                        foreach (var uitem in unordered)
                            uitem.Order--;

                    _videoRepository.CommitTransaction();

                    if (unordered != null)
                        _videoRepository.Update(unordered);

                    // delete the video from the database
                    _videoRepository.Delete(video);
                }
                catch (Exception ex)
                {
                    // TODO: remove the sources from storage if new
                    throw new BusinessException("Failed to remove video information.", ex);
                }

                ts.Complete();
            }

            return new JsonNetResult("success");
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateCategory(VideoCategoryModel model)
        {
            var category = Mapper.Map<VideoCategoryModel, VideoCategory>(model);
            _categoryRepository.Insert(category);
            Mapper.Map(category, model);

            return new JsonNetResult(model);
        }

        [HttpPut]
        [Authorize]
        public ActionResult UpdateCategory(VideoCategoryModel model)
        {
            var category = Mapper.Map<VideoCategoryModel, VideoCategory>(model);
            try
            {
                _categoryRepository.Update(category);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Failed to update category.", ex);
            }

            Mapper.Map(category, model);

            return new JsonNetResult(model);
        }

        [HttpDelete]
        [Authorize]
        public ActionResult RemoveCategory(int id)
        {
            var category = _categoryRepository.FindByID(id);

            if (category == null)
                throw new BusinessException("Category not found. It might have been already removed.");

            try
            {
                _categoryRepository.Delete(category);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Failed to remove the category. Please remove all the videos first.", ex);
            }

            return new JsonNetResult("success");
        }

        [Authorize]
        public ActionResult UpdateVideoOrder()
        {
            throw new NotImplementedException();
        }

    }
}