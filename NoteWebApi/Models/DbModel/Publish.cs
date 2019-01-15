using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWepApi.Models.DbModel
{
    public class Publish
    {
        public int PublishId { get; set; }
        public string PublisherUserName { get; set; }
        public string PublisherName { get; set; }
        public int PublisherId { get; set; }
        public DateTime PublishTime { get; set; }
        public DateTime? PublishModifiedTime { get; set; }
        public string PublishSef { get; set; }
        public int PublishIsActive { get; set; }
        public string PublishTitle { get; set; }
        public string PublishContent { get; set; }
       



    }
}