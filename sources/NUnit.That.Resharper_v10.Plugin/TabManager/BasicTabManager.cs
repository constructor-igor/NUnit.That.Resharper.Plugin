using System;
using JetBrains.ReSharper.Resources.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NUnit.That.Resharper_v10.Plugin.TabManager
{
    public abstract class BasicTabManager : ITabManager
    {
        // 
        // http://www.mztools.com/articles/2014/MZ2014017.aspx
        //

        private readonly IVsOutputWindowPane m_outputWindowPane;
        protected BasicTabManager(Guid tabGuid, string tabCaption)
        {
            const int VISIBLE = 1;
            const int DO_NOT_CLEAR_WITH_SOLUTION = 0;

            // Get the output window
            Lazy<IVsOutputWindow> outputWindow = Shell.Instance.GetComponent<Lazy<IVsOutputWindow>>();

            int hr = outputWindow.Value.CreatePane(tabGuid, tabCaption, VISIBLE, DO_NOT_CLEAR_WITH_SOLUTION);

            // Get the pane
            //IVsOutputWindowPane outputWindowPane;
            hr = outputWindow.Value.GetPane(tabGuid, out m_outputWindowPane);            
        }
        #region ITabManager
        public ITabManager OutputString(string text, params object[] parameters)
        {
            if (m_outputWindowPane != null)
            {
                m_outputWindowPane.Activate();
                m_outputWindowPane.OutputString(String.Format(text, parameters));
            }
            return this;
        }
        public ITabManager OutputLine(string text, params object[] parameters)
        {
            return OutputString(text + Environment.NewLine, parameters);
        }
        #endregion
    }
}