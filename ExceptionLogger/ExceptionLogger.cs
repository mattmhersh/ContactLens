using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Management;

namespace Utilities
{
  /// <summary>
  /// Abstract class for logging errors to different output devices, primarily for use in Windows Forms applications
  /// </summary>
  public abstract class LoggerImplementation
  {
    /// <summary>Logs the specified error.</summary>
    /// <param name="error">The error to log.</param>
    public abstract void LogError(string error);
  }

  /// <summary>
  /// Class to log unhandled exceptions
  /// </summary>
  public class ExceptionLogger
  {
    /// <summary>
    /// Creates a new instance of the ExceptionLogger class
    /// </summary>
    public ExceptionLogger()
    {
      Application.ThreadException +=
        new System.Threading.ThreadExceptionEventHandler(OnThreadException);
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);
      loggers = new List<LoggerImplementation>();
    }

    private List<LoggerImplementation> loggers;

    /// <summary>
    /// Adds a logger implementation to the list of used loggers.
    /// </summary>
    /// <param name="logger">The logger to add.</param>
    public void AddLogger(LoggerImplementation logger)
    {
      loggers.Add(logger);
    }

    delegate void LogExceptionDelegate(Exception e);

    private void HandleException(Exception e)
    {
      //if (MessageBox.Show("An unexpected error occurred - " + e.Message +
      //  ". Do you wish to log the error?", "Error", MessageBoxButtons.YesNo) == DialogResult.No)
      //  return;
        
      LogExceptionDelegate logDelegate = new LogExceptionDelegate(LogException);
      logDelegate.BeginInvoke(e, new AsyncCallback(LogCallBack), null);
    }

    // Event handler that will be called when an unhandled
    // exception is caught
    private void OnThreadException(object sender, ThreadExceptionEventArgs e)
    {
      // Log the exception to a file
      HandleException(e.Exception);
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      HandleException((Exception)e.ExceptionObject);
    }

    private void LogCallBack(IAsyncResult result)
    {
      AsyncResult asyncResult = (AsyncResult)result;
      LogExceptionDelegate logDelegate = (LogExceptionDelegate)asyncResult.AsyncDelegate;
      if (!asyncResult.EndInvokeCalled)
      {
        logDelegate.EndInvoke(result);
      }
    }

    private string GetExceptionTypeStack(Exception e)
    {
      if (e.InnerException != null)
      {
        StringBuilder message = new StringBuilder();
        message.AppendLine(GetExceptionTypeStack(e.InnerException));
        message.AppendLine("   " + e.GetType().ToString());
        return (message.ToString());
      }
      else
      {
        return "   " + e.GetType().ToString();
      }
    }

    private string GetExceptionMessageStack(Exception e)
    {
      if (e.InnerException != null)
      {
        StringBuilder message = new StringBuilder();
        message.AppendLine(GetExceptionMessageStack(e.InnerException));
        message.AppendLine("   " + e.Message);
        return (message.ToString());
      }
      else
      {
        return "   " + e.Message;
      }
    }

    private string GetExceptionCallStack(Exception e)
    {
      if (e.InnerException != null)
      {
        StringBuilder message = new StringBuilder();
        message.AppendLine(GetExceptionCallStack(e.InnerException));
        message.AppendLine("--- Next Call Stack:");
        message.AppendLine(e.StackTrace);
        return (message.ToString());
      }
      else
      {
        return e.StackTrace;
      }
    }

    private static TimeSpan GetSystemUpTime()
    {
      //PerformanceCounter upTime = new PerformanceCounter("System", "System Up Time");
      //upTime.NextValue();
      //return TimeSpan.FromSeconds(upTime.NextValue());
        return GetUptime();
    }

  /// 
  /// Obtains the OS uptime
  /// 
  /// TimeSpan object that contains the uptime
  private static TimeSpan GetUptime()
 
  {
       //timespan object to store the result value
      
      TimeSpan uptimeTs = new TimeSpan();
      
       //management objects to interact with WMI
      
      ManagementClass management = new ManagementClass("Win32_OperatingSystem");
      
      ManagementObjectCollection mngInstance = management.GetInstances();
      
       //loop throught the mngInstance
      
      foreach (ManagementObject mngObject in mngInstance)
         
      {
           //get the LastBootUpTime date parsed
          
          DateTime lastBootUp = ParseCIMDateTime(mngObject["LastBootUpTime"].ToString());
          
          //check it value is not DateTime.MinValue
          
          if (lastBootUp != DateTime.MinValue)
              
          {
               //get the diff between dates
              
              uptimeTs = DateTime.Now - lastBootUp;
              
          }
          
      }
      
       //return the uptime TimeSpan
      
      return uptimeTs;
  }

/// 
  /// 
  /// 
  /// The CIM_DateTime format is represented by the following string: 
  /// yyyy MM dd hh mm ss.mmm mmm UTC
  /// 2007 01 12 20 45 59.115 081+060
  /// 
  /// Input CIM_DateTime string
 /// DatTime object that represents the CIM_DateTIme input string
 private static DateTime ParseCIMDateTime(string wmiDate)
 {
     //datetime object to store the return value
     DateTime date = DateTime.MinValue;
     
     //check date integrity
    if (wmiDate != null && wmiDate.IndexOf('.') != -1)
    {
         //obtain the date with miliseconds
         string tempDate = wmiDate.Substring(0, wmiDate.IndexOf('.') + 4);
 
        //check the lenght
        if (tempDate.Length == 18)
         {
            //extract each date component
            int year = Convert.ToInt32(tempDate.Substring(0, 4));
            int month = Convert.ToInt32(tempDate.Substring(4, 2));
            int day = Convert.ToInt32(tempDate.Substring(6, 2));
             int hour = Convert.ToInt32(tempDate.Substring(8, 2));
             int minute = Convert.ToInt32(tempDate.Substring(10, 2));
             int second = Convert.ToInt32(tempDate.Substring(12, 2));
            int milisecond = Convert.ToInt32(tempDate.Substring(15, 3));
 
            //compose the new datetime object
             date = new DateTime(year, month, day, hour, minute, second, milisecond);        
         }
     }
 
     //return datetime
     return date;
 }

      // use to get memory available
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private class MEMORYSTATUSEX 
    { 
      public uint dwLength; 
      public uint dwMemoryLoad; 
      public ulong ullTotalPhys; 
      public ulong ullAvailPhys; 
      public ulong ullTotalPageFile; 
      public ulong ullAvailPageFile; 
      public ulong ullTotalVirtual; 
      public ulong ullAvailVirtual; 
      public ulong ullAvailExtendedVirtual; 
      
      public MEMORYSTATUSEX() 
      { 
        this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX)); 
      } 
    }
    
    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

      /// <summary>writes exception details to the registered loggers</summary>
      /// <param name="exception">The exception to log.</param>
      /// <param name="strCustomMessage"></param>
      public void LogException(Exception exception, string strCustomMessage)
    {
      StringBuilder error = new StringBuilder();

      error.AppendLine("Error Message:       " + strCustomMessage);
      error.AppendLine("");
      error.AppendLine("");
      error.AppendLine("Application:       " + Application.ProductName);
      error.AppendLine("Version:           " + Application.ProductVersion);
      error.AppendLine("Date:              " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
      error.AppendLine("Computer name:     " + SystemInformation.ComputerName);
      error.AppendLine("User name:         " + SystemInformation.UserName);
      error.AppendLine("OS:                " + Environment.OSVersion.ToString());
      error.AppendLine("Culture:           " + CultureInfo.CurrentCulture.Name);
      error.AppendLine("Resolution:        " + SystemInformation.PrimaryMonitorSize.ToString());
      error.AppendLine("System up time:    " + GetSystemUpTime());
      error.AppendLine("App up time:       " +
        (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString());

      MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX(); 
      if (GlobalMemoryStatusEx(memStatus)) 
      {
        error.AppendLine("Total memory:      " + memStatus.ullTotalPhys / (1024 * 1024) + "Mb");
        error.AppendLine("Available memory:  " + memStatus.ullAvailPhys / (1024 * 1024) + "Mb");
      }

      error.AppendLine("");

      error.AppendLine("Exception classes:   ");
      error.Append(GetExceptionTypeStack(exception));
      error.AppendLine("");
      error.AppendLine("Exception messages: ");
      error.Append(GetExceptionMessageStack(exception));

      error.AppendLine("");
      error.AppendLine("Stack Traces:");
      error.Append(GetExceptionCallStack(exception));
      error.AppendLine("");
      error.AppendLine("Loaded Modules:");
      Process thisProcess = Process.GetCurrentProcess();
      foreach (ProcessModule module in thisProcess.Modules)
      {
        error.AppendLine(module.FileName + " " + module.FileVersionInfo.FileVersion);
      }

      for (int i = 0; i < loggers.Count; i++)
      {
        loggers[i].LogError(error.ToString());
      }
    }

      /// <summary>writes exception details to the registered loggers</summary>
      /// <param name="exception">The exception to log.</param>
      public void LogException(Exception exception)
      {
          StringBuilder error = new StringBuilder();

          error.AppendLine("Application:       " + Application.ProductName);
          error.AppendLine("Version:           " + Application.ProductVersion);
          error.AppendLine("Date:              " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
          error.AppendLine("Computer name:     " + SystemInformation.ComputerName);
          error.AppendLine("User name:         " + SystemInformation.UserName);
          error.AppendLine("OS:                " + Environment.OSVersion.ToString());
          error.AppendLine("Culture:           " + CultureInfo.CurrentCulture.Name);
          error.AppendLine("Resolution:        " + SystemInformation.PrimaryMonitorSize.ToString());
          error.AppendLine("System up time:    " + GetSystemUpTime());
          error.AppendLine("App up time:       " +
            (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString());

          MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
          if (GlobalMemoryStatusEx(memStatus))
          {
              error.AppendLine("Total memory:      " + memStatus.ullTotalPhys / (1024 * 1024) + "Mb");
              error.AppendLine("Available memory:  " + memStatus.ullAvailPhys / (1024 * 1024) + "Mb");
          }

          error.AppendLine("");

          error.AppendLine("Exception classes:   ");
          error.Append(GetExceptionTypeStack(exception));
          error.AppendLine("");
          error.AppendLine("Exception messages: ");
          error.Append(GetExceptionMessageStack(exception));

          error.AppendLine("");
          error.AppendLine("Stack Traces:");
          error.Append(GetExceptionCallStack(exception));
          error.AppendLine("");
          error.AppendLine("Loaded Modules:");
          Process thisProcess = Process.GetCurrentProcess();
          foreach (ProcessModule module in thisProcess.Modules)
          {
              error.AppendLine(module.FileName + " " + module.FileVersionInfo.FileVersion);
          }

          for (int i = 0; i < loggers.Count; i++)
          {
              loggers[i].LogError(error.ToString());
          }
      }
  }
}


