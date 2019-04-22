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
    public class GiangVienModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<GiangVien> GiangVienCollection;
        public GiangVienModel()
        {
            mongoClient = new MongoClient(new configWEB().connectionstring);
            var db = mongoClient.GetDatabase("DangKyHocPhan");
            GiangVienCollection = db.GetCollection<GiangVien>("GiangVien");
        }
        public List<GiangVien> findAll()
        {
            return GiangVienCollection.AsQueryable<GiangVien>().ToList();
        }
        public GiangVien find(string id)
        {
            var GiangVienId = new ObjectId(id);
            return GiangVienCollection.AsQueryable<GiangVien>().SingleOrDefault(a => a.Id == GiangVienId);
        }
        public void create(GiangVien GiangVien)
        {
            GiangVienCollection.InsertOne(GiangVien);
        }
        public void update(GiangVien GiangVien)
        {
            GiangVienCollection.UpdateOne(
                Builders<GiangVien>.Filter.Eq("_id", ObjectId.Parse(GiangVien.Id.ToString())),
                Builders<GiangVien>.Update
                    .Set("tenGiangVien", GiangVien.TenGiangVien)
                );
        }
        public void delete(String id)
        {
            GiangVienCollection.DeleteOne(Builders<GiangVien>.Filter.Eq("_id", ObjectId.Parse(id)));
        }

    }
}