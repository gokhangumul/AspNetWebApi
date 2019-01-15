using NoteWepApi.Helper;
using NoteWepApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NoteWepApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/share")]
    public class ShareController : ApiController
    {

        /// <summary>
        /// Kullanıcıyla paylaşılan notlar
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("usershare")]
        public IHttpActionResult GetShare()
        {

            int id = UserInf.GetUser();
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                try
                {

                    if(db.SHARES.Any(x=>x.FromUserıd==id || x.ToUserId == id))
                    {

                        var result = db.SHARES.Where(x => x.ToUserId == id && x.IsActive==1 && x.NOTE.isActive==1).
                         Select(x => new Models.DbModel.Share
                         {
                             Id = x.NotId,
                             NoteTitle = x.NOTE.NoteTitle,
                             NoteContent = x.NOTE.NoteContent,
                             NoteDescription = x.NOTE.NoteDescription,
                             NoteCreatedDate = x.NOTE.NoteCreatedDate,
                             NoteSefLink = x.NOTE.NoteSefLink,
                             privateNoteCategoryId = x.NOTE.privateNoteCategoryId,
                             NoteCategoryId = x.NOTE.NoteCategoryId,
                             NoteUserId = x.NOTE.NoteUserId,
                             isActive = x.NOTE.isActive,
                             NoteModifiedDate = x.NOTE.NoteModifiedDate,
                             NoteModifiedId = x.NOTE.NoteModifiedId,
                             CategoryName = x.NOTE.SYSTEMCATEGOR.Name,
                             PCategoryName = x.NOTE.CATEGOR.CategoryName,
                             ModifiedName=x.NOTE.USER1.Name,
                             FromUserName=x.USER1.Name
                             
                         }).OrderByDescending(x => x.NoteCreatedDate).ToList();
                        if (result.Count!=0)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            return NotFound();
                        }


                    }
                    else
                    {
                        return NotFound();
                    }


                }
                catch(Exception e)
                {
                    return BadRequest(e.Message);
                }


            }




        }
       

        /// <summary>
        /// Kullanıcın diğer kullanıcılarla paylaştığı notlar
        /// </summary>
        
        [HttpGet]
        [Route("usershareh")]
        public IHttpActionResult GetUserShareh()
        {
            int id = UserInf.GetUser();
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                try
                {

                    if (db.SHARES.Any(x =>x.FromUserıd == id || x.ToUserId == id ))
                    {

                        var result = db.SHARES.Where(x => x.FromUserıd == id && x.IsActive == 1 && x.NOTE.isActive==1).
                         Select(x => new Models.DbModel.Share
                         {
                             Id = x.NotId,
                             NoteTitle = x.NOTE.NoteTitle,
                             NoteContent = x.NOTE.NoteContent,
                             NoteDescription = x.NOTE.NoteDescription,
                             NoteCreatedDate = x.NOTE.NoteCreatedDate,
                             NoteSefLink = x.NOTE.NoteSefLink,
                             privateNoteCategoryId = x.NOTE.privateNoteCategoryId,
                             NoteCategoryId = x.NOTE.NoteCategoryId,
                             NoteUserId = x.NOTE.NoteUserId,
                             isActive = x.NOTE.isActive,
                             NoteModifiedDate = x.NOTE.NoteModifiedDate,
                             NoteModifiedId = x.NOTE.NoteModifiedId,
                             CategoryName = x.NOTE.SYSTEMCATEGOR.Name,
                             PCategoryName = x.NOTE.CATEGOR.CategoryName,
                             ModifiedName = x.NOTE.USER1.Name,
                             FromUserName = x.USER1.Name

                         }).OrderByDescending(x => x.NoteCreatedDate).GroupBy(x=>x.Id).ToList();
                        var gidecek = result.Select(q => q.First());
                        if (gidecek.ToList().Count==0)
                        {
                            return BadRequest();
                        }
                        else
                        {
                            return Ok(gidecek);
                        }


                    }
                    else
                    {
                        return NotFound();
                    }


                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }


            }
        }
        [HttpGet]
        [Route("shus/{notid}")]
        public IHttpActionResult ShareUsers(int notid)
        {
            int id = UserInf.GetUser();
            
            using (MynoteDBEntities db = new MynoteDBEntities())
            {


                var result = db.SHARES.Where(x => x.FromUserıd == id && x.NotId==notid && x.IsActive==1).Select(x => new Models.DbModel.ShUs
                {
                    UserId = x.ToUserId,
                    UserName = x.USER.UserName,
                    Name = x.USER.Name,
                    ShareDate = x.Share_CreatedDate,
                    NotId=x.NotId

                }).OrderByDescending(x => x.ShareDate).ToList();
                if (result.Count != 0)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
               


            }

        }
        [HttpPut]
        [Route("deleteshare/{userid}/{notid}")]
        public IHttpActionResult DeleteShare(int userid,int notid)
        {
            using(MynoteDBEntities db=new MynoteDBEntities())
            {
                var result = db.SHARES.FirstOrDefault(x => x.ToUserId == userid && x.NotId == notid);
                result.IsActive = 0;
                int save = db.SaveChanges();
                if (save != 0)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }
    }
}
