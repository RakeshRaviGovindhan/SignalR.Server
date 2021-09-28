using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SignalR.Model.Base
{
    [BsonDiscriminator]
    public abstract class BaseModel
    {
        protected BaseModel()
        {
            Date = DateTime.UtcNow;
        }

        [BsonId]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
    }
}
