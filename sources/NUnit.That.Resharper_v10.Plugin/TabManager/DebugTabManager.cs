using System;

namespace NUnit.That.Resharper_v10.Plugin.TabManager
{
    public class DebugTabManager : BasicTabManager
    {
        protected DebugTabManager()
            : base(new Guid("2B73056F-D80E-459C-BD1A-8FFADA19E105"), "NUnit.That.Debug")
        {
        }

        public static ITabManager Create()
        {
#if DEBUG
            return new DebugTabManager();
#endif
            return new DummyTabManager();
        }
    }

    internal class DummyTabManager : ITabManager
    {
        #region ITabManager
        public ITabManager OutputString(string text, params object[] parameters)
        {
            return this;
        }
        public ITabManager OutputLine(string text, params object[] parameters)
        {
            return this;
        }
        #endregion
    }
}