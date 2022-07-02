using InstagramProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstagramProject.ViewModels
{
    public class Save_imageVM
    {
        public int ImageID { get; set; }
        public string image_post { get; set; }
        public Nullable<int> UserID { get; set; }

    }

    public class CommentsVM
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public Save_imageVM Save_images { get; set; }
        public SignupVM Signup { get; set; }
    }

    public class SignupVM
    {
        public int Id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string password { get; set; }
        public string ProfileImage { get; set; }
        public FriendsVM friends { get; set; }

    }
    public class FriendsVM
    {
        public int Id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int User_ID { get; set; }

    }

}
