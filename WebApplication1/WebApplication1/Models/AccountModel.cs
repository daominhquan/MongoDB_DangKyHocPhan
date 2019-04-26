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
    public class AccountModel
    {
        private MongoClient mongoClient;
        private IMongoCollection<Account> accountCollection;
        public AccountModel()
        {
            mongoClient = new MongoClient(new configWEB().connectionstring);
            var db = mongoClient.GetDatabase("DangKyHocPhan");

            accountCollection = db.GetCollection<Account>("account");
        }
        public List<Account> findAll()
        {
            return accountCollection.AsQueryable<Account>().ToList();
        }
        public Account find(string id)
        {
            var accountId = new ObjectId(id);
            return accountCollection.AsQueryable<Account>().SingleOrDefault(a => a.Id == accountId);
        }
        public Account find_username(string username)
        {
            return accountCollection.AsQueryable<Account>().SingleOrDefault(a => a.Username == username);
        }

        public void create(Account account)
        {
            accountCollection.InsertOne(account);
        }
        public void update(Account account)
        {
            accountCollection.UpdateOneAsync(
                Builders<Account>.Filter.Eq("_id", ObjectId.Parse(account.Id.ToString())),
                Builders<Account>.Update
                    .Set("Username", account.Username)
                    .Set("Password", account.Password)
                    .Set("Fullname", account.Fullname)
                    .Set("Status", account.Status)
                    .Set("id_LopHoc", account.id_LopHoc)
                    .Set("HocPhanDaDangKy", account.HocPhanDaDangKy)
                );
        }
        public void updateHocPhanDaDangKy(Account account,IClientSessionHandle session)
        {
            accountCollection.UpdateOne(session,
                Builders<Account>.Filter.Eq("_id", ObjectId.Parse(account.Id.ToString())),
                Builders<Account>.Update
                    .Set("Username", account.Username)
                    .Set("Password", account.Password)
                    .Set("Fullname", account.Fullname)
                    .Set("Status", account.Status)
                    .Set("id_LopHoc", account.id_LopHoc)
                    .Set("HocPhanDaDangKy", account.HocPhanDaDangKy)
                );
        }

        public void delete(String id)
        {
            accountCollection.DeleteOne(Builders<Account>.Filter.Eq("_id", ObjectId.Parse(id)));
        }

    }
}