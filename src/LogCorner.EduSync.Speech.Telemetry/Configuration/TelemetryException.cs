using System;
using System.Runtime.Serialization;

[Serializable]
public class TelemetryException : Exception
{
    public TelemetryException(string message) : base(message)
    {
    }

    protected TelemetryException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}