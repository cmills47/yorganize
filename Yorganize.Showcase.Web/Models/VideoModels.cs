using System;

namespace Yorganize.Showcase.Web.Models
{
    public class VideoModel
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public VideoCategoryModel Category { get; set; }
    }

    public class VideoSourceModel
    {
        public int ID { get; set; }
        public string Format { get; set; }
        public string Url { get; set; }
        public VideoModel Video { get; set; }
    }

    public class VideoCategoryModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}