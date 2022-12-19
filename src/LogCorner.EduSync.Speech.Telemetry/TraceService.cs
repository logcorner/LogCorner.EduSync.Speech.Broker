using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LogCorner.EduSync.Speech.Telemetry
{
    public class TraceService : ITraceService
    {
        public void AddActivityToHeader(Activity activity, IDictionary<string, string> props, TextMapPropagator propagator)
        {
            if (activity != null)
            {
                propagator.Inject(new PropagationContext(activity.Context, Baggage.Current), props,
                    InjectContextIntoHeader);
            }
        }

        public void AddActivityToHeader(Activity activity, IDictionary<string, byte[]> props, TextMapPropagator propagator)
        {
            if (activity != null)
            {
                propagator.Inject(new PropagationContext(activity.Context, Baggage.Current), props,
                    InjectContextIntoHeader);
            }
        }

        private void InjectContextIntoHeader(IDictionary<string, string> props, string key, string value)
        {
            props[key] = value;
        }

        private void InjectContextIntoHeader(IDictionary<string, byte[]> props, string key, string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value ?? string.Empty);
            props.Add(key, bytes);
        }

        public void SetActivityTags(Activity activity, IDictionary<string, object> tags)
        {
            foreach (var item in tags)
            {
                activity?.SetTag(item.Key, item.Value);
            }
        }

        public IEnumerable<string> ExtractTraceContextFromBasicProperties(IDictionary<string, string> headers, string key)
        {
            if (headers.TryGetValue(key, out var value))
            {
                return new[] { value };
            }

            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> ExtractTraceContextFromBasicProperties(IDictionary<string, byte[]> props, string key)
        {
            if (props.TryGetValue(key, out var value))
            {
                var stringValue = Encoding.UTF8.GetString(value);
                return new[] { stringValue };
            }

            return Enumerable.Empty<string>();
        }
    }
}