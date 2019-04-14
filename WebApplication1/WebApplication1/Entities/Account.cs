using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Entities
{
    public class Account
    {

        [BsonId]
        public ObjectId Id
        {
            get;
            set;
        }
        [Required(ErrorMessage = "không thể để trống")]
        [BsonElement("username")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Tài khoản tối thiểu 4 ký tự")]
        public string Username
        {
            get; set;
        }
        [Required(ErrorMessage = "không thể để trống")]
        [BsonElement("password")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "mật khẩu tối thiểu 4 ký tự")]
        public string Password
        {
            get; set;
        }
        [BsonElement("fullname")]
        public string Fullname
        {
            get; set;
        }
        [BsonElement("status")]
        public bool Status
        {
            get; set;
        }
    }



}