using System;
using System.Runtime.Serialization;

namespace LogCorner.EduSync.Speech.ElasticSearch;

[Serializable]
public class ElasticSearchException : Exception
{
    public ElasticSearchException(string message) : base(message)
    {
    }

    protected ElasticSearchException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}