//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoteWepApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FILE
    {
        public int Id { get; set; }
        public int PublishId { get; set; }
        public int UserId { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int IsActive { get; set; }
    
        public virtual PUBLICATION PUBLICATION { get; set; }
        public virtual USER USER { get; set; }
    }
}
