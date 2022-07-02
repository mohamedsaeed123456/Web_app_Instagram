using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using InstagramProject.Models;
using InstagramProject.ViewModels;

namespace InstagramProject.Controllers
{
    public class HomeController : Controller
    {
        InstagramEntities13 db = new InstagramEntities13();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult signup()
        {
            return View();
        }
        public ActionResult HomePage(int ID)
        {
            if (Session["id"] != null)
            {
                var sav = db.Save_image.Where(x =>x.Signup.Friends.FirstOrDefault().Signup1.Friends1.FirstOrDefault().User_request_toID==ID);
                return View(sav);


            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ProfilePage(int id)
        {
            if (Session["id"] != null)
            {
                var SIGN = db.Signups.Where(x => x.Id == id).ToList().FirstOrDefault();
                var image_save = db.Save_image.Where(x => x.UserID == id).ToList();
                var model = new Signup_SaveImage { sign = SIGN ,image = image_save };

                if ((int)Session["id"] != id)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(model);
                }

            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult Profiles(int ID)
        {
            if (Session["id"] != null)
            {

                if ((int)Session["id"]==ID)
                {
                    return Redirect(Url.Action("ProfilePage", "Home") + "?id=" + Session["id"]);

                }
                else {
                    int Id = (int)Session["id"];
                    var friends = db.Friends.Where(x => x.User_requestID == ID && x.Signup1.Friends1.FirstOrDefault().User_request_toID == Id|| x.User_requestID == Id && x.Signup1.Friends1.FirstOrDefault().User_request_toID == ID).ToList();
                    var signup = db.Signups.Where(a => a.Id == ID).FirstOrDefault();

                    //Get Result Exam marks detail as per student ID  
                    var images = (from a in db.Save_image
                                  where a.UserID == ID
                                  select a).ToList();


                    if (friends != null)
                    {
                        //Output set to ViewModel  
                       
                        var model = new Signup_SaveImage { sign = signup , friend = friends, image = images };
                        return View(model);
                    }
                    else {
                        return null;
                    }
                }
            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
      
        public ActionResult FriendList(int ID)
        {
            if (Session["id"] != null)
            {
                var friends = db.Friends.Where(x=>x.User_requestID ==ID && x.IsAccept == true || x.User_request_toID==x.Signup1.Id && x.IsAccept == true);
                if ((int)Session["id"] != ID)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(friends);

                }

            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }



        public ActionResult EditProfile(int id)
        {
            if (Session["id"] != null)
            {
                
                var edit = db.Signups.Where(r => r.Id == id).FirstOrDefault();
                if ((int)Session["id"] != id)
                {
                    return HttpNotFound();

                }
                else
                {
                    return View(edit);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

       


        [HttpGet]
        public ActionResult Sign()
        {
            var sign = db.Signups.ToList();
            return View(sign);
        }
        [HttpPost]
        public ActionResult Sign(Signup sig)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(sig.ImageFile.FileName);
                string extension = Path.GetExtension(sig.ImageFile.FileName);
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".PNG")
                {
                    
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    sig.ProfileImage = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    sig.ImageFile.SaveAs(fileName);
                    db.Signups.Add(sig);
                    db.SaveChanges();
                    ModelState.Clear();
                    return Json(new { result = 1 });
                }
                else {
                    return Json(new { result = 0 });
                }
            }
            return Json(new { result = 0 });
        }
       

        [HttpGet]
        public ActionResult Login()
        {
            var Login = db.Signups.ToList();
            return View(Login);
        }
        [HttpPost]
        public ActionResult Login(Signup log)
        {
            var obj = (from userlist in db.Signups
                       where userlist.email == log.email && userlist.password == log.password
                       select new
                       {
                           userlist.Id,
                           userlist.firstname,
                           userlist.lastname,
                           userlist.email,
                           userlist.mobile,
                           userlist.password,
                           userlist.ProfileImage
                       }
                      ).ToList();
            if (obj.FirstOrDefault() != null)
            {
                    Session["id"] = obj.FirstOrDefault().Id;
                    Session["firstname"] = obj.FirstOrDefault().firstname;
                    Session["lastname"] = obj.FirstOrDefault().lastname;
                    Session["email"] = obj.FirstOrDefault().email;
                    Session["mobile"] = obj.FirstOrDefault().mobile;
                    Session["password"] = obj.FirstOrDefault().password;
                    Session["image"] = obj.FirstOrDefault().ProfileImage;
                    return Json(new { result = 1, X = obj.FirstOrDefault().Id });
            }
            else
                {
                    return Json(new { result = 0 });
                }
        }
                   
        [HttpPost]
        public ActionResult Edit(Signup edi)
        {
            if (ModelState.IsValid)
            {
                if (edi.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(edi.ImageFile.FileName);
               
                    string extension = Path.GetExtension(edi.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    edi.ProfileImage = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    edi.ImageFile.SaveAs(fileName);
                    db.Entry(edi).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { result = 1 });
                }
                else {
                    edi.ProfileImage=(string)Session["image"];
                    db.Entry(edi).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { result = 1 });
                }
                
            } 

            return null;
               
                
            
        }
        [HttpPost]
        public ActionResult Save_image(Save_image save)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(save.Imagefile.FileName);
                string extension = Path.GetExtension(save.Imagefile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                save.image_post = "~/Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                save.Imagefile.SaveAs(fileName);
                db.Save_image.Add(save);
                db.SaveChanges();
                ModelState.Clear();
                return Json(new { result = 1 });
            }
            return Json(new { result = 0 });
        }

       
        [HttpGet]
        public ActionResult Friends(string searchString)
        {
            if (ModelState.IsValid)
            {
                var sign = from p in db.Signups select p;
                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    sign = sign.Where(p => p.firstname.Equals(searchString) || p.lastname.Equals(searchString) || p.email.Equals(searchString));
                }
                return View(sign);
            }
            return null;
        }

        public ActionResult Images_Comment(int id) {
            var comm = db.Save_image.Find(id);
            return View(comm);
        }

        public PartialViewResult GetComments(int ImageID)
        {
            IQueryable <CommentsVM> comments = db.Comments.Where(c => c.Save_image.ImageID == ImageID)
                                     .Select(c => new CommentsVM
                                     {
                                         Id = c.Id,
                                         CommentText = c.CommentText,
                                         Signup= new SignupVM 
                                         {
                                             Id = c.Id,
                                             firstname = c.Signup.firstname,
                                             lastname = c.Signup.lastname,
                                             ProfileImage = c.Signup.ProfileImage
                                         }
                                     }).AsQueryable();

            return PartialView("~/Views/Shared/_MyComments.cshtml", comments);
        }

        [HttpPost]
        public ActionResult AddComment(CommentsVM comment, int ImageID)
        {
            //bool result = false;  
            Comment commentEntity = null;
            int userId = (int)Session["id"];

            var user = db.Signups.FirstOrDefault(u => u.Id == userId);
            var post = db.Save_image.FirstOrDefault(p => p.ImageID == ImageID);

            if (comment != null)
            {

                commentEntity = new Comment
                {
                    CommentText = comment.CommentText,
                };


                if (user != null && post != null)
                {
                    post.Comments.Add(commentEntity);
                    user.Comments.Add(commentEntity);

                    db.SaveChanges();
                    //result = true;  
                }
            }

            return RedirectToAction("GetComments", "Home", new { ImageID = ImageID });
        }


        public string LikeThis(int id)
        {
            Save_image art = db.Save_image.FirstOrDefault(x => x.ImageID == id);
           
                art.Likes++;
                Like like = new Like();
                like.ImageID = id;
                like.UserID = (int)Session["id"];
                like.IsLike = true;
                db.Likes.Add(like);
                db.SaveChanges();

            
           

            return art.Likes.ToString();
        }
        public string UnlikeThis(int id)
        {
            Save_image art = db.Save_image.FirstOrDefault(x => x.ImageID == id);
            if (User.Identity.IsAuthenticated || Session["email"] != null)
            {
                int userid = Convert.ToInt32(Session["id"]);

                Like l = db.Likes.FirstOrDefault(x => x.ImageID == id && x.UserID == userid);
                art.Likes--;
                db.Likes.Remove(l);
                db.SaveChanges();

            }
            return art.Likes.ToString();
        }


        [HttpPost]
        public ActionResult Add_friend(Friend friend)
        {
            if (ModelState.IsValid)
            {           
                db.Friends.Add(friend);
                db.SaveChanges();
                return Json(new { result = 1 });
            }
            return Json(new { result = 0 });
        }


        public ActionResult Notification(int ID)
        {
            if (Session["id"] != null)
            {
                var friends = db.Friends.Where(x => x.User_request_toID == ID);
                if ((int)Session["id"] != ID)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(friends);
                }

            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public string Accept(int id)
        {
            Friend friend = db.Friends.FirstOrDefault(x => x.Id == id);

            friend.IsAccept = true;
            Accept accept = new Accept();
            accept.FriendID = id;
            accept.userID = friend.User_request_toID;
            db.Accepts.Add(accept);
            db.SaveChanges();
            return friend.IsAccept.ToString();
        }
        public string Reject(int id)
        {
            Friend friend = db.Friends.FirstOrDefault(x => x.Id == id);
            if (User.Identity.IsAuthenticated || Session["email"] != null)
            {
                int userid = Convert.ToInt32(Session["id"]);

                Accept a = db.Accepts.FirstOrDefault(x => x.FriendID == id && x.userID == userid);
                friend.IsAccept = false;
                if (db.Accepts.Count()>0) {
                    db.Accepts.Remove(a);
                    db.Friends.Remove(friend);
                }
                else {
                    db.Friends.Remove(friend);
                }
                db.SaveChanges();

            }
            return friend.IsAccept.ToString();
        }




    }
}