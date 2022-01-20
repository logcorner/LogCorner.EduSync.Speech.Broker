using System.Collections.Generic;
using System.Diagnostics;

namespace LogCorner.EduSync.Speech.Telemetry
{
    public interface ITraceService
    {
        Activity StartActivity(string activityName);

        void InjectContextIntoHeader(Activity activity, IDictionary<string, object> headers);

        Activity StartActivity(string activityName, IDictionary<string, object> props);

        void AddActivityTags(Activity activity, IDictionary<string, object> tags);

        void AddActivityEvent(Activity activity, string eventName, IDictionary<string, object> tags);
    }
}