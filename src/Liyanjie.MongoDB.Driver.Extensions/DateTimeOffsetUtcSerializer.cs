using System;

namespace MongoDB.Bson.Serialization.Serializers
{
    public class DateTimeOffsetUtcSerializer : StructSerializerBase<DateTimeOffset>
    {
        DateTimeOffsetUtcSerializer serializer;
        public DateTimeOffsetUtcSerializer Instance => serializer ??= new DateTimeOffsetUtcSerializer();

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTimeOffset value)
        {
            var bsonWriter = context.Writer;
            bsonWriter.WriteDateTime(BsonUtils.ToMillisecondsSinceEpoch(value.UtcDateTime));
        }

        public override DateTimeOffset Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonReader = context.Reader;
            return new DateTimeOffset(new BsonDateTime(bsonReader.ReadDateTime()).ToUniversalTime());
        }
    }
}
