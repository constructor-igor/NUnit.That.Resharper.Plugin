using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.Decompiler.Ast;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.UI.ActionsRevised;
using JetBrains.Util;

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
            return new ProcessStartInfo("explorer.exe",
                path.ExistsFile ? string.Format(@"/select,""{0}""", path) : string.Format(@"""{0}""", path.Directory));
        }
        [CanBeNull]
        private static FileSystemPath GetPathByContext([NotNull] IDataContext context)
        {
            //return null;
            ITreeNode selectedExpression = context.GetData(JetBrains.ReSharper.Psi.Services.DataConstants.SELECTED_EXPRESSION); //, it should give you the current ITreeNode.
            //IStringLiteralOwner stringExpression = selectedExpression as IStringLiteralOwner;
            IStringLiteralOwner stringExpression = selectedExpression as IStringLiteralOwner;
            if (stringExpression != null)
            {
                string stringValue = (string) stringExpression.ConstantValue.Value;
                if (File.Exists(stringValue))
                {
                    return FileSystemPath.CreateByCanonicalPath(stringValue);
                }
             }
            return null;
            //throw new NotImplementedException();
//            var statement = context.GetSelectedTreeNode<IStatement>(context);
//
//            ProjectsContext projectModelElement = context.Projects();
//            if (projectModelElement.Project!=null)
//                return projectModelElement.Project.GetOutputDirectory();
//            return null;
        }
    }
}