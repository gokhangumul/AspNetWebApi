using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWepApi.Models.DbModel
{
    public class CommentViewModel
    {
        public IEnumerable<Comment> Comments { get; set; }
        public Publish Publish { get; set; }
    }
}