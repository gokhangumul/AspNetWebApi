﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MynoteDBEntities : DbContext
    {
        public MynoteDBEntities()
            : base("name=MynoteDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CATEGOR> CATEGORS { get; set; }
        public virtual DbSet<FILE> FILES { get; set; }
        public virtual DbSet<FRIEND> FRIENDS { get; set; }
        public virtual DbSet<IMAGE> IMAGES { get; set; }
        public virtual DbSet<NOTE> NOTES { get; set; }
        public virtual DbSet<PUBLICATION> PUBLICATIONS { get; set; }
        public virtual DbSet<PUBLICATIONSCOMMENT> PUBLICATIONSCOMMENTS { get; set; }
        public virtual DbSet<SHARE> SHARES { get; set; }
        public virtual DbSet<SYSTEMCATEGOR> SYSTEMCATEGORs { get; set; }
        public virtual DbSet<USER> USERS { get; set; }
    }
}
