using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWepApi.Models.DbModel
{
    public class Share
    {
        public int Id { get; set; }
        public string NoteTitle { get; set; }
        public string NoteDescription { get; set; }
        public string NoteContent { get; set; }
        public DateTime NoteCreatedDate { get; set; }
        public DateTime? NoteModifiedDate { get; set; }
        public int? NoteModifiedId { get; set; }
        public int? NoteCategoryId { get; set; }
        public int NoteUserId { get; set; }
        public string NoteSefLink { get; set; }
        public int isActive { get; set; }
        public int? privateNoteCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string PCategoryName { get; set; }
        public string ModifiedName { get; set; }
        public string FromUserName { get; set; }
        
    }

}