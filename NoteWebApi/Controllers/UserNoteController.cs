using NoteWepApi.Filter;
using NoteWepApi.Helper;
using NoteWepApi.Models;
using NoteWepApi.Provider;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace NoteWepApi.Controllers
{
    [RoutePrefix("api/note")]
    public class UserNoteController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("usernote")]
        public IHttpActionResult GetNote()
        {
            int id = UserInf.GetUser();
            try
            {
                using (MynoteDBEntities db = new MynoteDBEntities())
                {

                   var result = db.NOTES.Where(x=>x.NoteUserId==id && x.isActive==1).
                          Select(x => new Models.DbModel.Note
                          {
                             Id= x.Id,
                             NoteTitle= x.NoteTitle,
                             NoteContent= x.NoteContent,
                             NoteDescription= x.NoteDescription,
                             NoteCreatedDate= x.NoteCreatedDate,
                             NoteSefLink= x.NoteSefLink,
                             privateNoteCategoryId= x.privateNoteCategoryId,
                             NoteCategoryId= x.NoteCategoryId,
                             Name=x.USER.Name,
                             NoteUserId= x.NoteUserId,
                             isActive= x.isActive,
                             NoteModifiedDate=x.NoteModifiedDate,
                             NoteModifiedId= x.NoteModifiedId,
                             CategoryName=x.SYSTEMCATEGOR.Name,
                             PCategoryName=x.CATEGOR.CategoryName
                          }).OrderByDescending(x => x.NoteCreatedDate).ToList();
                 
                    
                    if (result.Count !=0)
                    {
                        return Ok(result);
                        
                    }
                    else
                    {
                        return BadRequest("Bu id'ye ait not bulunmamaktadır");
                    }


                }
            }
            catch(Exception e)
            {

                return BadRequest(e.Message);

            }


        }
       

        [Authorize]
        [HttpPost]
        [Route("createnote")]
        public IHttpActionResult CreateNote(NOTE note)
        {
            
            int id= UserInf.GetUser();  
            try
            {
                using (MynoteDBEntities db = new MynoteDBEntities())
                {

                        note.NoteUserId = id;
                        db.NOTES.Add(note);
                        int feed = db.SaveChanges();
                        if (feed != 0)
                        {
                            return Ok();

                        }
                        return BadRequest();

                    
                   
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);


            }

        }

        [Authorize]
        [HttpGet]
        [Route("updategetnote/{seflink}")]
        public IHttpActionResult GetUpdateNote(string seflink)
        {

            int id = UserInf.GetUser();
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                var result = db.NOTES.Where(x => x.NoteSefLink == seflink)
                    .Select(x => new
                    { x.Id, x.NoteContent, x.NoteTitle, x.NoteUserId, x.NoteDescription, x.NoteCreatedDate,
                     x.NoteModifiedId, x.NoteModifiedDate, x.NoteSefLink,x.NoteCategoryId,x.privateNoteCategoryId}).ToList();
                if (result.Count == 0)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(result.FirstOrDefault());
                }


            }


        }

        [Authorize]
        [HttpPut]
        [Route("updatenote/{seflink}")]
        public IHttpActionResult UpdateNote(string seflink,[FromBody]NOTE note)
        {
            try
            {
                int id = UserInf.GetUser();
                using(MynoteDBEntities db=new MynoteDBEntities())
                {
                    var gnote = db.NOTES.FirstOrDefault(x => x.NoteSefLink == seflink);
                    if (gnote != null)
                    {
                        if (note.NoteCategoryId != null)
                        {
                            gnote.NoteTitle = note.NoteTitle;
                            gnote.NoteDescription = note.NoteDescription;
                            gnote.NoteContent = note.NoteContent;
                            gnote.NoteCategoryId = note.NoteCategoryId;
                            gnote.NoteModifiedDate = note.NoteModifiedDate;
                            gnote.NoteModifiedId = id;
                            int result = db.SaveChanges();
                            if (result != 0)
                            {
                                return Ok();
                            }
                            else
                            {
                                return BadRequest();
                            }

                        }
                        else
                        {
                            gnote.NoteTitle = note.NoteTitle;
                            gnote.NoteDescription = note.NoteDescription;
                            gnote.NoteContent = note.NoteContent;
                            gnote.privateNoteCategoryId = note.privateNoteCategoryId;
                            gnote.NoteModifiedDate = note.NoteModifiedDate;
                            gnote.NoteModifiedId = id;
                            int result = db.SaveChanges();
                            if (result != 0)
                            {
                                return Ok();
                            }
                            else
                            {
                                return BadRequest();
                            }
                        }


                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);

            }


        }
        [Authorize]
        [HttpPut]
        [Route("deletenote/{id}")]
        public IHttpActionResult DeleteNote(int id)
        {
            using(MynoteDBEntities db=new MynoteDBEntities())
            {
                try
                {
                    var result = db.NOTES.FirstOrDefault(x => x.Id==id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                       result.isActive = 0;
                       int save= db.SaveChanges();
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
                catch (Exception e)
                {

                    return BadRequest(e.Message);
                }

            }
        }

        [Authorize]
        [HttpPost]
        [Route("share")]
        public IHttpActionResult Share([FromBody] SHARE share)
        {
            int fromuserID = UserInf.GetUser();
            try
            {
                using (MynoteDBEntities db = new MynoteDBEntities())
                {
                    if (fromuserID == share.ToUserId)
                    {
                        return BadRequest();
                    }
                    if(db.SHARES.Any(x=>x.ToUserId==share.ToUserId && x.FromUserıd == fromuserID && x.NotId==share.NotId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        
                        share.FromUserıd = fromuserID;
                        share.Share_CreatedDate = DateTime.Now;
                        share.IsActive = 1;
                        db.SHARES.Add(share);
                        int feed = db.SaveChanges();
                        if (feed != 0)
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
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpGet]
        [Route("findpdfcontent/{id}")]
        public IHttpActionResult Pdf(int id)
        {


            using (MynoteDBEntities db = new MynoteDBEntities())
            {

                var result = db.NOTES.Where(x => x.Id == id).Select(x => new Models.DbModel.Pdf
                {
                    Title = x.NoteTitle,
                    Description = x.NoteDescription,
                    Content = x.NoteContent
                }).ToList();
                if (result != null)
                {
                    return Ok(result.FirstOrDefault());
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [Authorize]
        [Support]
        [HttpPost]
        [Route("imagecreate/{notid}")]
        public async Task<IHttpActionResult> ImageCreate(int notid)
        {
            int userid = UserInf.GetUser();
            List<string> path = new List<string>();      
            var fileuploadPath = HttpContext.Current.Server.MapPath("~/UploadedFiles");
            var multiFormDataStreamProvider = new MultiFileUploadProvider(fileuploadPath);
            await Request.Content.ReadAsMultipartAsync(multiFormDataStreamProvider);
            path = multiFormDataStreamProvider.FileData.Select(x => x.LocalFileName).ToList();
            foreach(var item in path)
            {
                using (MynoteDBEntities db = new MynoteDBEntities())
                {

                    IMAGE img = new IMAGE
                    {
                        ImagePath = item,
                        ImageName = Path.GetFileName(item),
                        NotId = notid,
                        UserId = userid,
                        CreatedDate = DateTime.Now
                    };
                    db.IMAGES.Add(img);
                    db.SaveChanges();
                   
                        
                    


                }


            }

            return Ok();

        }
        [Authorize]
        [HttpGet]
        [Route("getimage/{id}")]
        public IHttpActionResult GetImage(int id)
        {
            List<byte[]> img = new List<byte[]>();
            byte[] imgb;
            using(MynoteDBEntities db=new MynoteDBEntities())
            {
                var result = db.IMAGES.Where(x => x.NotId == id).Select(x => new { x.Id, x.ImageName, x.ImagePath, x.CreatedDate }).ToList();
                if (result.Count != 0)
                {
                    try
                    {

                        foreach (var item in result)
                        {
                            imgb = File.ReadAllBytes(HttpContext.Current.Server.MapPath("~/UploadedFiles/" + item.ImageName));
                            img.Add(imgb);
                        }

                        return Ok(img);
                       
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }
                }
                else
                {
                    return BadRequest();
                }

            }



        }


    }
}
