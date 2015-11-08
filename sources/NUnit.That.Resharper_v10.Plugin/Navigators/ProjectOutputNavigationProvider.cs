using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;
using JetBrains.UI.ActionsRevised;
using JetBrains.Util;

namespace NUnit.That.Resharper_v10.Plugin.Navigators
{
    [ActionHandler(null)]
    public class ProjectOutputNavigation : ContextNavigationActionBase<ProjectOutputNavigationProvider>
    {
    }

    [ContextNavigationProvider]
    public class ProjectOutputNavigationProvider : INavigateFromHereProvider
    {
        #region Implementation of IWorkflowProvider<ContextNavigation,out NavigationActionGroup>
        public IEnumerable<ContextNavigation> CreateWorkflow(IDataContext dataContext)
        {
            var path = GetPathByContext(dataContext);
            if (path != null)
            {
                ProcessStartInfo processStartInfo = GetProcessStartInfo(path);
                if (processStartInfo != null)
                {
                        yield return new ContextNavigation(String.Format("Open &Project Output Folder ({0})", path.Name), null, NavigationActionGroup.Other,
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
            ProjectsContext projectModelElement = context.Projects();
            if (projectModelElement.Project != null)
                return projectModelElement.Project.GetOutputFilePath();
            return null;
        }
    }
}