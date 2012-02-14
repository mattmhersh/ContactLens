using System.Diagnostics;

namespace Utilities
{
  /// <summary>Logs errors to the application event log</summary>
  public class EventLogLogger : LoggerImplementation
  {
    /// <summary>Logs the specified error.</summary>
    /// <param name="error">The error to log.</param>
    public override void LogError(string error)
    {

        // Source cannot already exist before creating the log.
        if (EventLog.SourceExists("Contact Replenishment"))
        {
            EventLog.DeleteEventSource("Contact Replenishment");
        }

        // Logs and Sources are created as a pair.
        EventLog.CreateEventSource("Contact Replenishment", "Contact Replenishment Log");
        // Associate the EventLog component with the new log.
        var log = new EventLog { Log = "Contact Replenishment Log", Source = "Contact Replenishment" };

        //EventLog log = new EventLog("Application");
        //EventLog log = new EventLog("Contact Replenishment Log");
        //log.Source = Assembly.GetExecutingAssembly().ToString();
        log.WriteEntry(error, EventLogEntryType.Error);
    }
  }
}
