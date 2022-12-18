namespace LogCorner.EduSync.Speech.Telemetry.Configuration;

public static class Helper
{
    public static int ParseInt(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0;
        if (int.TryParse(value, out var result))
        {
            return result;
        }

        throw new TelemetryException($"{value} cannot be parsed to an integer value");
    }
}