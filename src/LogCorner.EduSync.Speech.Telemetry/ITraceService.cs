using OpenTelemetry.Context.Propagation;
using System.Collections.Generic;
using System.Diagnostics;

namespace LogCorner.EduSync.Speech.Telemetry
{
    public interface ITraceService
    {
        void SetActivityTags(Activity activity, IDictionary<string, object> tags);

        IEnumerable<string> ExtractTraceContextFromBasicProperties(IDictionary<string, string> headers, string key);

        IEnumerable<string> ExtractTraceContextFromBasicProperties(IDictionary<string, byte[]> props, string key);

        void AddActivityToHeader(Activity activity, IDictionary<string, string> props, TextMapPropagator propagator);

        void AddActivityToHeader(Activity activity, IDictionary<string, byte[]> props, TextMapPropagator propagator);
    }
}