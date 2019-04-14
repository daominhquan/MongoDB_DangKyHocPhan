using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Entities
{
    public class GiangVien
    {
        [BsonId]
        public ObjectId Id
        {
            get; set;
        }
        [Required(ErrorMessage = "không thể để trống")]
        [BsonElement("tenGiangVien")]
        public string TenGiangVien
        {
            get; set;
        }
    }



}