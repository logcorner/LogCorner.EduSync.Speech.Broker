using Microsoft.Extensions.Configuration;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LogCorner.EduSync.Speech.Telemetry
{
    public class TraceService : ITraceService
    {
        private readonly IConfiguration _configuration;

        public TraceService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Activity StartActivity(string activityName)
        {
            var activitySourceName = _configuration["OpenTelemetry:SourceName"];

            if (string.IsNullOrWhiteSpace(activitySourceName))
            {
                throw new ArgumentNullException(nameof(activitySourceName));
            }

            if (string.IsNullOrWhiteSpace(activityName))
            {
                throw new ArgumentNullException(nameof(activityName));
            }

            ActivitySource activitySource = new(activitySourceName);

            return activitySource.StartActivity(activityName, ActivityKind.Producer);
        }

        public void InjectContextIntoHeader(Activity activity, IDictionary<string, object> headers)
        {
            headers ??= new Dictionary<string, object>();
            TextMapPropagator propagator = Propagators.DefaultTextMapPropagator;
            propagator.Inject(new PropagationContext(activity.Context, Baggage.Current), headers, InjectContextIntoHeader);
        }

        private void InjectContextIntoHeader(IDictionary<string, object> headers, string key, string value)
        {
            headers ??= new Dictionary<string, object>();
            headers[key] = value;
        }

        public Activity StartActivity(string activityName, IDictionary<string, object> headers)
        {
            var activitySourceName = _configuration["OpenTelemetry:SourceName"];

            if (string.IsNullOrWhiteSpace(activitySourceName))
            {
                throw new ArgumentNullException(nameof(activitySourceName));
            }

            if (string.IsNullOrWhiteSpace(activityName))
            {
                throw new ArgumentNullException(nameof(activityName));
            }
            TextMapPropagator propagator = new TraceContextPropagator();
            var parentContext = propagator.Extract(default, headers, ExtractTraceContextFromHeader);
            Baggage.Current = parentContext.Baggage;
            ActivitySource activitySource = new(activitySourceName);
            var activity = activitySource.StartActivity(activityName, ActivityKind.Consumer,
                parentContext.ActivityContext);

            return activity;
        }

        private static IEnumerable<string> ExtractTraceContextFromHeader(IDictionary<string, object> headers, string key)
        {
            if (headers.TryGetValue(key, out var value))
            {
                if (value is byte[] bytes)
                {
                    return new[] { Encoding.UTF8.GetString(bytes) };
                }
                var stringValue = value as string;
                return new[] { stringValue };
            }

            return Enumerable.Empty<string>();
        }

        public void AddActivityTags(Activity activity, IDictionary<string, object> tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException(nameof(tags));
            }
            foreach (var (key, value) in tags)
            {
                activity?.SetTag(key, value);
            }
        }

        public void AddActivityEvent(Activity activity, string eventName, IDictionary<string, object> tags)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }
            activity?.AddEvent(new ActivityEvent(eventName, tags: new ActivityTagsCollection(tags)));
        }
    }
}