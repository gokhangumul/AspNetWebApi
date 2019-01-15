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
    [RoutePrefix("api/category")]
    public class CategoryController : ApiController
    {
        [Authorize]
        [HttpGet]
        [Route("usercategory")]
        public IHttpActionResult UserCategory()
        {
            int id = UserInf.GetUser();
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                try
                {
                  var result = db.CATEGORS.Where(x => x.UserId == id && x.isActive==1).Select(x => new { x.Id, x.UserId, x.CategoryName,x.Seflink }).ToList();
                    if (result.Count != 0)
                    {
                        return Ok(result);
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
        [Authorize]
        [HttpGet]
        [Route("systemcategory")]
        public IHttpActionResult SystemCategory()
        {
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                try
                {
                    var result = db.SYSTEMCATEGORs.Select(x => new { x.Id, x.Name }).ToList();
                    if (result.Count != 0)
                    {
                        return Ok(result);
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

        
        [Authorize]
        [HttpPost]
        [Route("createcategory")]
        public IHttpActionResult CreateCategory(CATEGOR category)
        {
            int id = UserInf.GetUser();
            try
            {
                using (MynoteDBEntities db = new MynoteDBEntities())
                {
                    category.UserId = id;
                    db.CATEGORS.Add(category);
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
            catch(Exception e)
            {

                return BadRequest(e.Message);
            }

        }

        [Authorize]
        [HttpGet]
        [Route("getupdate/{sef}")]
        public IHttpActionResult Getupdate(string sef)
        {
            int id = UserInf.GetUser();
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                try
                {
                    var result = db.CATEGORS.Where(x => x.UserId == id && x.Seflink==sef).Select(x => new { x.Id, x.UserId, x.CategoryName, x.Seflink }).ToList();
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
        [Authorize]
        [HttpPut]
        [Route("getdelete/{sef}")]
        public IHttpActionResult Delete(string sef,[FromBody]CATEGOR category)
        {
            try
            {
                int id = UserInf.GetUser();
                using (MynoteDBEntities db = new MynoteDBEntities())
                {

                    var result = db.CATEGORS.FirstOrDefault(x => x.UserId == id && x.Seflink == sef);
                    if (result != null)
                    {
                        result.isActive = category.isActive;
                        int feed=db.SaveChanges();
                        if (feed != 0)
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
                        return BadRequest();
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
        [Route("updatecategory/{sef}")]
        public IHttpActionResult Update(string sef, [FromBody]CATEGOR category)
        {
            try
            {
                int id = UserInf.GetUser();
                using (MynoteDBEntities db = new MynoteDBEntities())
                {

                    var result = db.CATEGORS.FirstOrDefault(x => x.UserId == id && x.Seflink == sef);
                    if (result != null)
                    {
                        result.CategoryName = category.CategoryName;
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
}
