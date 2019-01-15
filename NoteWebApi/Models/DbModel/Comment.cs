using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWepApi.Models.DbModel
{
    public class Comment
    {
     
        public int CommentId { get; set; }
        public int CommenterId { get; set; }
        public string CommenterUserName { get; set; }
        public string CommenterName { get; set; }
        public DateTime CommentTime { get; set; }
        public int CommentIsActive { get; set; }
        public string CommentContent { get; set; }

    }
}