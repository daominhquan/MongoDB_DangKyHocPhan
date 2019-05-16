using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Entities
{
    public class HocPhan
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("id_LopHoc")]
        public string id_LopHoc { get; set; }

        [BsonElement("SiSo")]
        public int SiSo { get; set; }

        [BsonElement("LyThuyet")]
        public TietHoc LyThuyet { get; set; }

        [BsonElement("ThucHanh")]
        public TietHoc ThucHanh { get; set; }

        [BsonElement("DanhSachSinhVien")]
        public List<string> DanhSachSinhVien { get; set; }
        

        [BsonElement("Status")]
        public bool Status { get; set; }



    }
}