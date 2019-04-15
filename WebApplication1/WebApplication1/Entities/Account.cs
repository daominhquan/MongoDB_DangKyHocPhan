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
        [BsonElement("Username")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Tài khoản tối thiểu 4 ký tự")]
        public string Username
        {
            get; set;
        }
        [Required(ErrorMessage = "không thể để trống")]
        [BsonElement("Password")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "mật khẩu tối thiểu 4 ký tự")]
        public string Password
        {
            get; set;
        }
        [BsonElement("Fullname")]
        public string Fullname
        {
            get; set;
        }
        [BsonElement("id_LopHoc")]
        public string id_LopHoc
        {
            get; set;
        }

        [BsonElement("HocPhanDaDangKy")]
        public List<string> HocPhanDaDangKy
        {
            get; set;
        }
        [BsonElement("Status")]
        public bool Status
        {
            get; set;
        }
    }



}