using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Entities
{
    public class LopHoc
    {
        [BsonId]
        public ObjectId Id
        {
            get; set;
        }
        [Required(ErrorMessage = "không thể để trống")]
        [BsonElement("maLopHoc")]
        public string MaLopHoc
        {
            get; set;
        }
        [Required(ErrorMessage = "không thể để trống")]
        [BsonElement("tenLopHoc")]
        public string TenLopHoc
        {
            get; set;
        }
    }
}