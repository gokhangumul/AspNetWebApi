using NoteWepApi.Helper;
using NoteWepApi.Models;
using System;
using System.Linq;
using System.Web.Http;


namespace NoteWepApi.Controllers
{ 
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public IHttpActionResult RegisterUser(USER model)
        {

            if (model == null)
            {
                return BadRequest("Geçersiz kullanıcı");

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                PassManagement ps = new PassManagement();
                byte[] salt = ps.Hashing(model.Mail);
                string hashing = ps.HashPass(model.Hash, salt);
                model.Hash = hashing;
                model.RegisterDate = DateTime.Now;
                using (MynoteDBEntities ent = new MynoteDBEntities())
                {
                    ent.USERS.Add(model);
                    int result = ent.SaveChanges();
                    if (result != 0)
                    {
                        return Ok("Kullanıcı başarıyla kaydedilmiştir");
                    }
                    else
                    {
                        return BadRequest("Kullanıcı kaydı başarısız");
                    }

                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /*
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IHttpActionResult> LoginUser([FromBody]Login user)
        {
            if (user == null)
            {
                return BadRequest("Geçersiz kullanıcı");
            }

            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", user.Mail),
                new KeyValuePair<string, string>("password", user.Password)
            };
            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);

            try
            {
                using (HttpClient cl = new HttpClient())
                {
                    cl.BaseAddress = new Uri("http://localhost:49473");
                    cl.DefaultRequestHeaders.Clear();
                    cl.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var tokenservice = cl.PostAsync("/getToken", requestParamsFormUrlEncoded).Result;

                    if (tokenservice.StatusCode == HttpStatusCode.OK)
                    {
                        var responsestring = await tokenservice.Content.ReadAsStringAsync();
                        var serialize = new JavaScriptSerializer();
                        var responsedata = serialize.Deserialize<Dictionary<string, string>>(responsestring);
                        var token = responsedata["access_token"];
                        var token_type = responsedata["token_type"];
                        var expiress_in = responsedata["expires_in"];
                        TokenResult result = new TokenResult()
                        {
                            Token = token,
                            Token_Type = token_type,
                            Expires_in = expiress_in

                        };
                        return Ok(result);
                    }
                    else
                    {
                        return ResponseMessage(tokenservice);
                    }
                    
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }


        }
        */
        [AllowAnonymous]
        [HttpGet]
        [Route("mail/{mail}")]
        public IHttpActionResult UserMailCheck(string mail)
        {
            string checkmail = mail.Replace(",com", ".com");
            using (MynoteDBEntities db =new MynoteDBEntities())
            {
                if (db.USERS.Any(x=>x.Mail==checkmail))
                {
                    return BadRequest("Bu mail kullanımda");
                }
                else
                {
                    return Ok();
                }

            }


        }

        [Authorize]
        [HttpGet]
        [Route("updatemail/{mail}")]
        public IHttpActionResult UpdateUserMailCheck(string mail)
        {
            int id = UserInf.GetUser();
            string checkmail = mail.Replace(",com", ".com");
            using (MynoteDBEntities db = new MynoteDBEntities())
            {
                
                if (db.USERS.Any(x => x.Mail == checkmail))
                {
                    var result = db.USERS.FirstOrDefault(x => x.Mail == checkmail);
                    if (result.Id == id)
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
                    return Ok();
                }

            }

        }


        [AllowAnonymous]
        [HttpGet]
        [Route("username/{username}")]
        public IHttpActionResult UserNameCheck(string username)
        {
            try
            {
                using (MynoteDBEntities db = new MynoteDBEntities())
                {
                    if (db.USERS.Any(x => x.UserName == username))
                    {
                        return BadRequest("Bu kullanıcı adı kullanımda");
                    }
                    else
                    {
                        return Ok();
                    }

                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
           


        }

        [Authorize]
        [HttpGet]
        [Route("updateusername/{username}")]
        public IHttpActionResult UpdateUserNameCheck(string username)
        {
            int id = UserInf.GetUser();

            using (MynoteDBEntities db = new MynoteDBEntities())
            {

                if (db.USERS.Any(x => x.UserName == username))
                {
                    var result = db.USERS.FirstOrDefault(x => x.UserName == username);
                    if (result.Id == id)
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
                    return Ok();
                }

            }

        }

        [Authorize]
        [HttpGet]
        [Route("userid/{name}")]
        public IHttpActionResult GetUserId(string name)
        {
            try
            {

                using (MynoteDBEntities db = new MynoteDBEntities())
                {
                    var user = db.USERS.Where(x => x.UserName == name || x.Name == name).Select(x => new { x.Id }).ToList();
                    if (user.Count!=0)
                    {
                        return Ok(user.FirstOrDefault());
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
        [Route("getuserprofil")]
        public IHttpActionResult GeUserProfil()
        {
            int id = UserInf.GetUser();
            using(MynoteDBEntities db=new MynoteDBEntities())
            {
                try
                {
                    var result = db.USERS.Where(x => x.Id == id).Select(x => new { x.Id, x.UserName, x.Name, x.Mail, x.Hash }).ToList();
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
        [Route("updateprofil")]
        public IHttpActionResult UpdateProfil([FromBody] USER user)
        {

            int id = UserInf.GetUser();
            using(MynoteDBEntities db=new MynoteDBEntities())
            {
                try
                {
                    var result = db.USERS.FirstOrDefault(x => x.Id == id);
                    if (result == null)
                    {
                        return BadRequest();
                    }
                    else
                    {
                        PassManagement ps = new PassManagement();
                        byte[] salt = ps.Hashing(user.Mail);
                        string hashing = ps.HashPass(user.Hash, salt);
                        result.Hash = hashing;
                        result.Mail = user.Mail;
                        result.UpdatedDate = DateTime.Now;
                        result.UserName = user.UserName;
                        result.Name = user.Name;
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



        }


    }
}
