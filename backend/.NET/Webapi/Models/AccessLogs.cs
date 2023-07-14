using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Webapi.Models
{
    public class AccessLogs
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public DateTimeOffset Time { get; set; }
        public string LogInfomation { get; set; }
    }
}
