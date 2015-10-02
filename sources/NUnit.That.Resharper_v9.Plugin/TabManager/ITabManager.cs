namespace NUnit.That.Resharper_v9.Plugin.TabManager
{
    public interface ITabManager
    {
        ITabManager OutputString(string text, params object[] parameters);
        ITabManager OutputLine(string text, params object[] parameters);
    }
}