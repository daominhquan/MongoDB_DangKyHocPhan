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

        [BsonElement("maMH")]
        public string MaMH { get; set; }

        [BsonElement("tenMonHoc")]
        public string TenMonHoc { get; set; }

        [BsonElement("maLop")]
        public string MaLop { get; set; }

        [BsonElement("siSo")]
        public int SiSo { get; set; }

        [BsonElement("danhSachSinhVien")]
        public List<Account> DanhSachSinhVien { get; set; }

        [BsonElement("lyThuyet")]
        public TietHoc LyThuyet { get; set; }

        [BsonElement("thucHanh")]
        public TietHoc ThucHanh { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }

        

    }



}