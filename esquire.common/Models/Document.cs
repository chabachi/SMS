using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esquire.common.Models
{
    public abstract class Document : IDocument
    {
        public Guid Id { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DateDeleted { get; set; }
    }

    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        Guid Id { get; set; }

        DateTime CreateDate { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
