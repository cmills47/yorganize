﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Yorganize.Showcase.Web.Models
{
    public class VideoModel
    {
        public Guid ID { get; set; }

        [Required]
        public string Title { get; set; }
        public string Slug { get; set; }
        public int Order { get; set; }

        [Required]
        public string Description { get; set; }
        public string SourceMP4Url { get; set; }
        public string SourceOGGUrl { get; set; }
        public string SourceWEBMUrl { get; set; }

        public int CategoryID { get; set; }
    }

    public class VideoCategoryModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class VideoListModel
    {
        public string Category { get; set; }
        public List<VideoModel> Videos { get; set; }
    }
}