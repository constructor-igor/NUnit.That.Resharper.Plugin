using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;
using JetBrains.ReSharper.Psi.CSharp.Impl.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.UI.ActionsRevised;
using JetBrains.Util;
using DataConstants = JetBrains.ReSharper.Psi.Services.DataConstants;

namespace NUnit.That.Resharper_v9.Plugin
{
    [ActionHandler(null)]
    public class FolderNavigation : ContextNavigationActionBase<FolderNavigationProvider>
    {
    }

    [ContextNavigationProvider]
    public class FolderNavigationProvider : INavigateFromHereProvider
    {
        #region INavigateFromHereProvider
        public IEnumerable<ContextNavigation> CreateWorkflow(IDataContext dataContext)
        {
            var path = GetPathByContext(dataContext);
            if (path != null)
            {
                ProcessStartInfo processStartInfo = GetProcessStartInfo(path);
                if (processStartInfo != null)
                {
                    yield return new ContextNavigation(String.Format("Open &Folder ({0})", path.Name), null, NavigationActionGroup.Other,
                        () =>
                        {
                            try
                            {
                                using (Process.Start(processStartInfo)) { }
                            }
                            catch (Exception e)
                            {
                                MessageBox.ShowError(e.Message);
                            }
                        });
                }
            }
        }
        #endregion
        private static ProcessStartInfo GetProcessStartInfo([NotNull] FileSystemPath path)
        {
            string fullPath = Path.GetFullPath(path.FullPath);
            return new ProcessStartInfo("explorer.exe",
                path.ExistsFile ? string.Format(@"/select,""{0}""", fullPath) : string.Format(@"""{0}""", path.Directory));
        }
        [CanBeNull]
        private static FileSystemPath GetPathByContext([NotNull] IDataContext context)
        {
            ITreeNode selectedExpression = context.GetData(DataConstants.SELECTED_EXPRESSION);
            return GetFilePath(selectedExpression);
        }

        public static FileSystemPath GetFilePath(ITreeNode selectedExpression)
        {
            try
            {
                CSharpGenericToken stringExpression;
                ICSharpLiteralExpression literalExpression = selectedExpression as ICSharpLiteralExpression;
                if (literalExpression != null)
                {
                    stringExpression = literalExpression.Literal as CSharpGenericToken;
                }
                else
                {
                    stringExpression = selectedExpression as CSharpGenericToken;
                }

                if (stringExpression != null)
                {
                    string stringLiteralValue = stringExpression.GetText();
                    TokenNodeType tokenType = stringExpression.GetTokenType();
                    switch (tokenType.ToString())
                    {
                        case "STRING_LITERAL_VERBATIM":
                            stringLiteralValue = stringLiteralValue.Substring(2, stringLiteralValue.Length - 3);
                            if (File.Exists(stringLiteralValue))
                            {
                                return FileSystemPath.CreateByCanonicalPath(stringLiteralValue);
                            }
                            break;
                        case "STRING_LITERAL_REGULAR":
                            stringLiteralValue = stringLiteralValue.Substring(1, stringLiteralValue.Length - 2);
                            if (File.Exists(stringLiteralValue))
                            {
                                return FileSystemPath.CreateByCanonicalPath(stringLiteralValue);
                            }
                            break;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}