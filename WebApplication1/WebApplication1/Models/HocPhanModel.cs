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
            mongoClient = new MongoClient(new configWEB().connectionstring);
            var db = mongoClient.GetDatabase("DangKyHocPhan");
            HocPhanCollection = db.GetCollection<HocPhan>("HocPhan");
        }
        public List<HocPhan> findAll()
        {
            return HocPhanCollection.AsQueryable<HocPhan>().ToList();
        }
        public HocPhan find(string id)
        {
            if (id == "" || id == null)
            {
                return null;
            }
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
                    .Set("id_LopHoc", HocPhan.id_LopHoc)
                    .Set("SiSo", HocPhan.SiSo)
                    .Set("LyThuyet", HocPhan.LyThuyet)
                    .Set("ThucHanh", HocPhan.ThucHanh)
                    .Set("DanhSachSinhVien", HocPhan.DanhSachSinhVien)
                    .Set("Status", HocPhan.Status)
                );
        }
        public void capnhathocphan(HocPhan HocPhan, IClientSessionHandle session)
        {
            HocPhanCollection.UpdateOne(session,

                Builders<HocPhan>.Filter.Eq("_id", ObjectId.Parse(HocPhan.Id.ToString())),
                Builders<HocPhan>.Update
                    .Set("id_LopHoc", HocPhan.id_LopHoc)
                    .Set("SiSo", HocPhan.SiSo)
                    .Set("LyThuyet", HocPhan.LyThuyet)
                    .Set("ThucHanh", HocPhan.ThucHanh)
                    .Set("DanhSachSinhVien", HocPhan.DanhSachSinhVien)
                    .Set("Status", HocPhan.Status)
                );
        }


        public void delete(String id)
        {
            HocPhanCollection.DeleteOne(Builders<HocPhan>.Filter.Eq("_id", ObjectId.Parse(id)));
        }


    }
}