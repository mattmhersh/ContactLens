using System;
using System.Net;
using System.Net.Mail;

namespace Utilities
{
  /// <summary>Sends error logs as emails</summary>
  public class EmailLogger : LoggerImplementation
  {
    /// <summary>Logs the specified error.</summary>
    /// <param name="error">The error to log.</param>
    public override void LogError(string error)
    {
      // check all properties have been set
      if (string.IsNullOrEmpty(emailFrom))
        throw new ArgumentException("EmailFrom has not been set");
      if (string.IsNullOrEmpty(emailTo))
        throw new ArgumentException("EmailTo has not been set");
      if (string.IsNullOrEmpty(EmailServer))
        throw new ArgumentException("EmailServer has not been set");

      MailMessage message = new MailMessage(emailFrom, emailTo, "Unhandled exception report", error);
      SmtpClient client = new SmtpClient(emailServer);
      // Add credentials if the SMTP server requires them.
      client.Credentials = CredentialCache.DefaultNetworkCredentials;
      client.Send(message);
    }

    private string emailServer;
    /// <summary>
    /// Specifies the email server that the exception information email will be sent via
    /// </summary>
    public string EmailServer
    {
      get
      {
        return emailServer;
      }
      set
      {
        emailServer = value;
      }
    }

    private string emailFrom;
    /// <summary>
    /// Specifies the email address that the exception information will be sent from
    /// </summary>
    public string EmailFrom
    {
      get
      {
        return emailFrom;
      }
      set
      {
        emailFrom = value;
      }
    }

    private string emailTo;
    /// <summary>
    /// Specifies the email address that the exception information will be sent to
    /// </summary>
    public string EmailTo
    {
      get
      {
        return emailTo;
      }
      set
      {
        emailTo = value;
      }
    }
  }
}
