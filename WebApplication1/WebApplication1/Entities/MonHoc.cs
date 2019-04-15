using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Entities
{
    public class MonHoc
    {
        [BsonId]
        public ObjectId Id
        {
            get; set;
        }
        [Required(ErrorMessage = "không thể để trống")]
        [BsonElement("MaMonHoc")]
        public string MaMonHoc
        {
            get; set;
        }
        [Required(ErrorMessage = "không thể để trống")]
        [BsonElement("TenMonHoc")]
        public string TenMonHoc
        {
            get; set;
        }
        [BsonElement("DanhSachHocPhan")]
        public List<HocPhan> DanhSachHocPhan
        {
            get; set;
        }
    }
}