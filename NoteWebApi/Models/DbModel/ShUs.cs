using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteWepApi.Models.DbModel
{
    public class ShUs
    {
        public int NotId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public DateTime ShareDate { get; set; }
    }
}