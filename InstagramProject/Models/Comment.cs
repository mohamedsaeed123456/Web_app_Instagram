//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InstagramProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> Image_ID { get; set; }
    
        public virtual Signup Signup { get; set; }
        public virtual Save_image Save_image { get; set; }
    }
}
