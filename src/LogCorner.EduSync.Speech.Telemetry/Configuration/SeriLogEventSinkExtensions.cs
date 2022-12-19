using Serilog;
using Serilog.Configuration;
using System;

namespace LogCorner.EduSync.Speech.Telemetry.Configuration
{
    public static class SeriLogEventSinkExtensions
    {
        public static LoggerConfiguration NewRelicLogEventSink(
            this LoggerSinkConfiguration loggerConfiguration,
            IFormatProvider formatProvider = null)
        {
            return loggerConfiguration.Sink(new SeriLogEventSink(formatProvider));
        }
    }
}