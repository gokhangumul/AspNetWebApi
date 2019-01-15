using NoteWepApi.Filter;
using NoteWepApi.Helper;
using NoteWepApi.Models;
using NoteWepApi.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
namespace NoteWepApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/publish")]
    public class PublishController : ApiController
    {

        [HttpGet]
        [Route("mypublish")]
        public IHttpActionResult GetMyPublish()
        {
            int id = UserInf.GetUser();
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                try
                {
                    var result = db.PUBLICATIONS.Where(x => x.PubUserId == id && x.isActive == 1).Select
                        (x => new { x.Id, x.PubTitle, x.PubUserId, x.PubContent, x.PubCreatedDate, x.PubModifiedDate, x.isActive, x.PubSefLink }).ToList();
                    if (result.Count != 0)
                    {
                        return Ok(result);
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
        [HttpPost]
        [Route("mypublishadd")]
        public IHttpActionResult AddMyPublish([FromBody]PUBLICATION publish)
        {
            int id = UserInf.GetUser();
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                try
                {
                    publish.PubUserId = id;
                    db.PUBLICATIONS.Add(publish);
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
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

            }

        }

        [HttpGet]
        [Route("mypublishgetupdate/{id}")]
        public IHttpActionResult GetMyPublishUpdate(int id)
        {
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                try
                {
                    var result = db.PUBLICATIONS.Where(x => x.Id == id).Select(x => new { x.Id, x.PubContent, x.PubTitle }).ToList();
                    if (result.Count != 0)
                    {
                        return Ok(result.FirstOrDefault());
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }
        [HttpPut]
        [Route("mypublishupdate/{id}")]
        public IHttpActionResult MyPublishUpdate([FromBody] PUBLICATION publish, int id)
        {
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                try
                {
                    var result = db.PUBLICATIONS.FirstOrDefault(x => x.Id == id);
                    if (result == null)
                    {
                        return BadRequest();
                    }

                    result.PubContent = publish.PubContent;
                    result.PubTitle = publish.PubTitle;
                    result.PubModifiedDate = DateTime.Now;
                    result.PubSefLink = publish.PubSefLink;
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
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }
        [HttpPut]
        [Route("mypublishdelete/{id}")]
        public IHttpActionResult DeleteMyPublish(int id)
        {
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                var result = db.PUBLICATIONS.FirstOrDefault(x => x.Id == id);
                result.isActive = 0;
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

        [HttpGet]
        [Route("friendpublish")]
        public IHttpActionResult GetFriendPublish()
        {

            HashSet<int> liste = new HashSet<int>();
            int id = UserInf.GetUser();
            try
            {
                using (MynoteDBEntities db = new MynoteDBEntities())
                {
                    var result = db.FRIENDS.Where(x => x.FromUserId == id || x.ToUserId == id).Select(x => new
                    {
                        x.Id,
                        x.FromUserId,
                        x.ToUserId,
                        x.StatusCode
                    }).ToList();
                    foreach (var item in result)
                    {
                        if (item.StatusCode == 1)
                        {
                            liste.Add(item.ToUserId);
                            liste.Add(item.FromUserId);
                        }
                    }
                    liste.Remove(id);
                    var pubresult = db.PUBLICATIONS.Where(x => liste.Contains(x.PubUserId) && x.isActive == 1)
                        .Select(x => new Models.DbModel.Publish
                        {
                            PublishId = x.Id,
                            PublisherId = x.PubUserId,
                            PublisherName = x.USER.Name,
                            PublisherUserName = x.USER.UserName,
                            PublishContent = x.PubContent,
                            PublishTitle = x.PubTitle,
                            PublishModifiedTime = x.PubModifiedDate,
                            PublishTime = x.PubCreatedDate,
                            PublishIsActive = x.isActive,
                            PublishSef = x.PubSefLink,
                        }).OrderByDescending(x => x.PublishTime).ToList();
                    if (pubresult.Count != 0)
                    {
                        return Ok(pubresult);
                    }
                    else
                    {
                        return NotFound();
                    }



                }


            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }



        }
        [Route("addcomment")]
        [HttpPost]
        public IHttpActionResult AddComment([FromBody] PUBLICATIONSCOMMENT comment)
        {
            int userid = UserInf.GetUser();
            comment.CommentUserId = userid;
            comment.CommentDate = DateTime.Now;
            comment.isActive = 1;
            try
            {
                using (MynoteDBEntities db = new MynoteDBEntities())
                {
                    db.PUBLICATIONSCOMMENTS.Add(comment);
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
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        [Route("getcomment/{pubid}")]
        [HttpGet]
        public IHttpActionResult GetComment(int pubid)
        {
            try
            {
                using (MynoteDBEntities db = new MynoteDBEntities())
                {
                    var presult = db.PUBLICATIONS.Where(x => x.Id == pubid).Select(x => new Models.DbModel.Publish
                    {
                        PublishContent = x.PubContent,
                        PublishTitle = x.PubTitle,
                        PublishId = x.Id
                    }).ToList();
                    var cresult = db.PUBLICATIONSCOMMENTS.Where(x => x.CommnetPubId == pubid && x.isActive == 1).Select(x => new Models.DbModel.Comment
                    {
                        CommentContent = x.CommentContent,
                        CommenterName = x.USER.Name,
                        CommenterUserName = x.USER.UserName,
                        CommentId = x.Id,
                        CommenterId = x.CommentUserId,
                        CommentTime = x.CommentDate



                    }).OrderByDescending(x => x.CommentTime).ToList();
                    Models.DbModel.CommentViewModel view = new Models.DbModel.CommentViewModel
                    {
                        Publish = presult.FirstOrDefault(),
                        Comments = cresult
                    };
                    if (cresult.Count != 0)
                    {
                        return Ok(view);
                    }
                    else
                    {
                        return BadRequest();
                    }

                }
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize]
        [Support]
        [HttpPost]
        [Route("publishfilecreate/{publishid}")]
        public async Task<IHttpActionResult> FilesCreate(int publishid)
        {
            int userid = UserInf.GetUser();
            List<string> path = new List<string>();
            var fileuploadPath = HttpContext.Current.Server.MapPath("~/UploadedFilesPublish");
            var multiFormDataStreamProvider = new MultiFileUploadProvider(fileuploadPath);
            await Request.Content.ReadAsMultipartAsync(multiFormDataStreamProvider);
            path = multiFormDataStreamProvider.FileData.Select(x => x.LocalFileName).ToList();
            foreach (var item in path)
            {
                using (MynoteDBEntities db = new MynoteDBEntities())
                {

                    FILE file = new FILE
                    {
                        PublishId = publishid,
                        FileName = Path.GetFileName(item),
                        UserId = userid,
                        CreatedTime = DateTime.Now,
                        FilePath = item,
                        IsActive = 1

                    };
                    db.FILES.Add(file);
                    db.SaveChanges();
                }


            }

            return Ok();

        }

        [Authorize]
        [HttpGet]
        [Route("getfiles/{id}")]
        public IHttpActionResult GetFiles(int id)
        {

            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                var result = db.FILES.Where(x => x.PublishId == id).Select(x => new { x.Id, x.FileName, x.FilePath, x.IsActive, x.UserId, x.PublishId }).ToList();
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

        
        [HttpGet]
        [Route("download/{id}")]
        public HttpResponseMessage DownloadFile(int id)
        {
            //Physical Path of Root Folder
            var rootPath = HttpContext.Current.Server.MapPath("~/UploadedFilesPublish");
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                var result = db.FILES.FirstOrDefault(x => x.Id == id);
                if (result == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
                else
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    byte[] file = File.ReadAllBytes(result.FilePath);
                    MemoryStream ms = new MemoryStream(file);
                    response.Content = new ByteArrayContent(file);
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = result.FileName
                    };
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    return response;
                }
            }

        }
    }
}