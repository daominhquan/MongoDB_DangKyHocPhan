using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Entities
{
    public class TietHoc
    {
        public int Thu { get; set; }
        public int TietBD { get; set; }
        public int SoTiet { get; set; }
        public string Phong { get; set; }
        public string GiangVien { get; set; }


        //[BsonId]
        //public ObjectId Id
        //{
        //    get; set;
        //}
        //[Required(ErrorMessage = "không thể để trống")]
        //[BsonElement("isThucHanh")]
        //public bool IsThucHanh { get; set; }

        //[Required(ErrorMessage = "không thể để trống")]
        //[BsonElement("thu")]
        //public int Thu { get; set; }

        //[Required(ErrorMessage = "không thể để trống")]
        //[BsonElement("tietBD")]
        //public int TietBD { get; set; }

        //[Required(ErrorMessage = "không thể để trống")]

        //[BsonElement("soTiet")]
        //public int SoTiet { get; set; }

        //[Required(ErrorMessage = "không thể để trống")]
        //[BsonElement("phong")]
        //public string Phong { get; set; }

        //[Required(ErrorMessage = "không thể để trống")]
        //[BsonElement("giangVien")]
        //public string GiangVien { get; set; }


    }



}