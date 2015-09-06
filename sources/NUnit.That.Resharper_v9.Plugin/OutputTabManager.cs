using System;
using JetBrains.ReSharper.Resources.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NUnit.That.Resharper_v9.Plugin
{
    public class OutputTabManager
    {
        private readonly Guid m_nUnitThatGuid = new Guid("D556397E-D292-4707-B924-5FFFD20802C7");
        // 
        // http://www.mztools.com/articles/2014/MZ2014017.aspx
        //
        public void OutputString(string text, params object[] parameters)
        {
            const int VISIBLE = 1;
            const int DO_NOT_CLEAR_WITH_SOLUTION = 0;

            // Get the output window
            JetBrains.Util.Lazy.Lazy<IVsOutputWindow> outputWindow = Shell.Instance.GetComponent<JetBrains.Util.Lazy.Lazy<IVsOutputWindow>>();

            int hr = outputWindow.Value.CreatePane(m_nUnitThatGuid, "NUnit.That", VISIBLE, DO_NOT_CLEAR_WITH_SOLUTION);

            // Get the pane
            IVsOutputWindowPane outputWindowPane;
            hr = outputWindow.Value.GetPane(m_nUnitThatGuid, out outputWindowPane);

            // Output the text
            if (outputWindowPane != null)
            {
                outputWindowPane.Activate();
                outputWindowPane.OutputString(String.Format(text, parameters));
            }
        }
    }
}