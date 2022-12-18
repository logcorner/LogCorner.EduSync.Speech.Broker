using System;
using System.Runtime.Serialization;

namespace LogCorner.EduSync.Speech.Telemetry.Configuration;

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