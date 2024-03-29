﻿using Serilog.Core;
using Serilog.Events;
using System;

namespace LogCorner.EduSync.Speech.Telemetry
{
    public class SeriLogEventSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;

        public SeriLogEventSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            var message = logEvent.RenderMessage(_formatProvider);
        }
    }
}