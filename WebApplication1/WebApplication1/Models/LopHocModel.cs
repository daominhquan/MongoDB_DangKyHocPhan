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
    public class LopHocModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<LopHoc> LopHocCollection;
        public LopHocModel()
        {
            mongoClient = new MongoClient(new configWEB().connectionstring);
            var db = mongoClient.GetDatabase("DangKyHocPhan");
            LopHocCollection = db.GetCollection<LopHoc>("LopHoc");
        }
        public List<LopHoc> findAll()
        {
            return LopHocCollection.AsQueryable<LopHoc>().ToList();
        }
        public LopHoc find(string id)
        {
            var LopHocId = new ObjectId(id);
            return LopHocCollection.AsQueryable<LopHoc>().SingleOrDefault(a => a.Id == LopHocId);
        }
        public void create(LopHoc LopHoc)
        {
            LopHocCollection.InsertOne(LopHoc);
        }
        public void update(LopHoc LopHoc)
        {
            LopHocCollection.UpdateOne(
                Builders<LopHoc>.Filter.Eq("_id", ObjectId.Parse(LopHoc.Id.ToString())),
                Builders<LopHoc>.Update
                    .Set("maLopHoc", LopHoc.MaLopHoc)
                    .Set("tenLopHoc", LopHoc.TenLopHoc)
                );
        }
        public void delete(String id)
        {
            LopHocCollection.DeleteOne(Builders<LopHoc>.Filter.Eq("_id", ObjectId.Parse(id)));
        }

    }
}