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
    [RoutePrefix("api/friend")]
    public class FriendController : ApiController
    {
    
        [HttpGet]
        [Route("getaddfriend/{username}")]
        public IHttpActionResult GetAddFriend(string username)
        {
            string tusername = username.Trim();
            int id = UserInf.GetUser();
            using(MynoteDBEntities db=new MynoteDBEntities())
            {
                try {
                    var result = db.USERS.FirstOrDefault(x => x.UserName == tusername);
                    if (result == null)
                    {
                        return Content(HttpStatusCode.NotFound, "Böyle bir kullanıcı bulunamadı");
                    }
                    else
                    {

                        if (id != result.Id)
                        {
                            var sonuc = db.FRIENDS.FirstOrDefault(x => (x.ToUserId == id && x.FromUserId == result.Id) || (x.ToUserId == result.Id && x.FromUserId == id));
                            if (sonuc == null)
                            {
                                var user = db.USERS.Where(x => x.Id == result.Id).Select(x => new { x.Id, x.Name, x.UserName }).ToList();
                                return Content(HttpStatusCode.Accepted, user.FirstOrDefault());
                            }
                            else
                            {
                                if (sonuc.StatusCode == 0)
                                {
                                    var user = db.USERS.Where(x => x.Id == result.Id).Select(x => new { x.Id, x.Name, x.UserName }).ToList();
                                    return Content(HttpStatusCode.Created, user.FirstOrDefault());
                                }
                                else
                                {
                                    var user = db.USERS.Where(x => x.Id == result.Id).Select(x => new { x.Id, x.Name, x.UserName }).ToList();
                                    return Content(HttpStatusCode.OK, user.FirstOrDefault());
                                }
                            }
                        }
                        else
                        {
                            return Content(HttpStatusCode.NoContent, "Kendini ekleyemezsin");
                        }

                    }

                } catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
               


            }

        }
        [HttpPost]
        [Route("adduserfriend/{id}")]
        public IHttpActionResult AddUserFriend(int id)
        {
            int userid = UserInf.GetUser();
            using(MynoteDBEntities db=new MynoteDBEntities())
            {
                try
                {
                    FRIEND friend = new FRIEND
                    {
                        FromUserId = userid,
                        ToUserId = id,
                        StatusCode = 0
                    };
                    db.FRIENDS.Add(friend);
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
                catch(Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

        [HttpGet]
        [Route("myfriend")]
        public IHttpActionResult MyFriend()
        {
            int id = UserInf.GetUser();
            HashSet<int> list = new HashSet<int>(); 
            using(MynoteDBEntities db=new MynoteDBEntities())
            {
                if(db.FRIENDS.Any(x=>x.ToUserId==id && x.StatusCode == 1))
                {
                    var result = db.FRIENDS.Where(x => x.ToUserId == id && x.StatusCode == 1).Select(x => new { x.FromUserId }).ToList();
                    foreach(var item in result)
                    {
                        list.Add(item.FromUserId);
                    }
                    
                }
                if(db.FRIENDS.Any(x=>x.FromUserId==id && x.StatusCode == 1))
                {
                    var result = db.FRIENDS.Where(x => x.FromUserId == id && x.StatusCode == 1).Select(x => new { x.ToUserId }).ToList();
                    foreach (var item in result)
                    {
                        list.Add(item.ToUserId);
                    }
                }
                list.Remove(id);
                if (list.Count != 0)
                {
                    var users = db.USERS.Where(x => list.Contains(x.Id)).Select(x => new { x.Id, x.UserName, x.Name }).ToList();
                    return Ok(users);
                }
                else
                {
                    return BadRequest();
                }
                
            }
        }

        [HttpGet]
        [Route("myfriendreq")]
        public IHttpActionResult GetMyFrienReq()
        {
            HashSet<int> list = new HashSet<int>();
            int id = UserInf.GetUser();
            using(MynoteDBEntities db=new MynoteDBEntities())
            {
                try
                {
                    var result = db.FRIENDS.Where(x => x.ToUserId == id && x.StatusCode==0).Select(x => new { x.FromUserId }).ToList();
                    foreach (var item in result)
                    {
                        list.Add(item.FromUserId);
                    }
                    if (list.Count != 0)
                    {
                        var request = db.USERS.Where(x => list.Contains(x.Id)).Select(x => new { x.Id, x.Name, x.UserName }).ToList();
                        return Ok(request);
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
        [Route("okfriendrequest/{id}")]
        public IHttpActionResult OkRequest(int id)
        {
            int userid = UserInf.GetUser();
            using(MynoteDBEntities db=new MynoteDBEntities())
            {
                try
                {
                    var friend = db.FRIENDS.FirstOrDefault(x => x.ToUserId == userid && x.FromUserId == id && x.StatusCode == 0);
                    if (friend == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        friend.StatusCode = 1;
                        int save = db.SaveChanges();
                        if(save!=0)
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
        [HttpDelete]
        [Route("deletefriend/{id}")]
        public IHttpActionResult DeleteMyFriend(int id)
        {
            int userid = UserInf.GetUser();
            using(MynoteDBEntities db=new MynoteDBEntities())
            {
                try
                {
                    var result = db.FRIENDS.FirstOrDefault(x => x.ToUserId == userid && x.FromUserId == id);
                    db.FRIENDS.Remove(result);
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
    }
}
