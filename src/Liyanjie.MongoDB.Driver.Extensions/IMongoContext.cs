namespace MongoDB.Driver
{
    public interface IMongoContext
    {
        IMongoClient MongoClient { get; }
    }
}
