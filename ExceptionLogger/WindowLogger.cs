using System.Windows.Forms;

namespace Utilities
{
  /// <summary>
  /// Logs errors to a window shown on screen
  /// </summary>
  public class WindowLogger : LoggerImplementation
  {
    /// <summary>
    /// Logs the specified error.
    /// </summary>
    /// <param name="error">The error to log.</param>
    public override void LogError(string error)
    {
      Form errorForm = new Form();
      if (Application.OpenForms.Count > 0)
      {
        errorForm.Width = Application.OpenForms[0].Width;
        errorForm.Height = Application.OpenForms[0].Height;
        errorForm.Left = Application.OpenForms[0].Left;
        errorForm.Top = Application.OpenForms[0].Top;
        errorForm.StartPosition = FormStartPosition.Manual;
        errorForm.TopLevel = true;
        errorForm.TopMost = true;
      }
      else
      {
        errorForm.Width = 600;
        errorForm.Height = 1000;
        errorForm.StartPosition = FormStartPosition.CenterScreen;
      }

      errorForm.Text = "An error has occured:";

      RichTextBox errorBox = new RichTextBox();
      errorForm.Controls.Add(errorBox);
      errorBox.Top = 10;
      errorBox.Left = 5;
      errorBox.Width = errorForm.Width - 20;
      errorBox.Height = errorForm.ClientRectangle.Height - 30 - errorBox.Top;
      errorBox.Text = error;
      errorBox.Font = new System.Drawing.Font("Courier New", 10);
      errorBox.ReadOnly = true;
      errorBox.WordWrap = false;
      errorBox.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
      errorBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

      Button errorOk = new Button();
      errorForm.Controls.Add(errorOk);
      errorOk.Top = errorForm.ClientRectangle.Height - 25;
      errorOk.Left = errorForm.ClientRectangle.Width - 5 - errorOk.Width;
      errorOk.Text = "&OK";
      errorOk.DialogResult = DialogResult.Cancel;
      errorOk.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
      errorOk.FlatStyle = FlatStyle.System;
      errorForm.CancelButton = errorOk;
      errorForm.AcceptButton = errorOk;

      errorForm.ShowDialog();
    }
  }
}
