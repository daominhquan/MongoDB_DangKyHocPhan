using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using WebApplication1.Entities;

namespace WebApplication1.Models
{
    public class HocPhanModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<HocPhan> HocPhanCollection;
        public HocPhanModel()
        {
            mongoClient = new MongoClient("mongodb://localhost");
            var db = mongoClient.GetDatabase("DangKyHocPhan");
            HocPhanCollection = db.GetCollection<HocPhan>("HocPhan");
        }
        public List<HocPhan> findAll()
        {
            return HocPhanCollection.AsQueryable<HocPhan>().ToList();
        }
        public HocPhan find(string id)
        {
            var HocPhanId = new ObjectId(id);
            return HocPhanCollection.AsQueryable<HocPhan>().SingleOrDefault(a => a.Id == HocPhanId);


        }
        public void create(HocPhan HocPhan)
        {
            HocPhanCollection.InsertOne(HocPhan);
        }
        public void update(HocPhan HocPhan)
        {
            HocPhanCollection.UpdateOne(
                Builders<HocPhan>.Filter.Eq("_id", ObjectId.Parse(HocPhan.Id.ToString())),
                Builders<HocPhan>.Update
                    .Set("maMH", HocPhan.MaMH)
                    .Set("tenMonHoc", HocPhan.TenMonHoc)
                    .Set("maLop", HocPhan.MaLop)
                    .Set("siSo", HocPhan.SiSo)
                    .Set("danhSachSinhVien", HocPhan.DanhSachSinhVien)
                    .Set("lyThuyet", HocPhan.LyThuyet)
                    .Set("thucHanh", HocPhan.ThucHanh)
                    .Set("status", HocPhan.Status)
                );
        }
        public void delete(String id)
        {
            HocPhanCollection.DeleteOne(Builders<HocPhan>.Filter.Eq("_id", ObjectId.Parse(id)));
        }

    }
}