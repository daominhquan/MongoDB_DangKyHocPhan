using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Entities
{
    public class Account
    {
        [BsonId]
        public ObjectId Id
        {
            get;
            set;
        }
        [BsonElement("username")]
        public string Username
        {
            get; set;
        }
        [BsonElement("password")]
        public string Password
        {
            get; set;
        }
        [BsonElement("fullname")]
        public string Fullname
        {
            get; set;
        }
        [BsonElement("status")]
        public bool Status
        {
            get; set;
        }
    }



}