using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Navigation.ContextNavigation;
using JetBrains.UI.ActionsRevised;
using JetBrains.Util;
using DataConstants = JetBrains.ProjectModel.DataContext.DataConstants;

namespace NUnit.That.Resharper_v9.Plugin
{
    [ActionHandler(null)]
    public class SourceFileNavigation : ContextNavigationActionBase<SourceFileNavigationProvider>
    {
    }

    [ContextNavigationProvider]
    public class SourceFileNavigationProvider : INavigateFromHereProvider
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
                    yield return new ContextNavigation("&Open Containing Folder", null, NavigationActionGroup.Other,
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
        [CanBeNull]
        private static ProcessStartInfo GetProcessStartInfo([NotNull] FileSystemPath path)
        {
            return new ProcessStartInfo("explorer.exe",
                path.ExistsFile ? string.Format(@"/select,""{0}""", path) : string.Format(@"""{0}""", path.Directory));
        }
        [CanBeNull]
        private static FileSystemPath GetPathByContext([NotNull] IDataContext context)
        {
            IProjectModelElement projectModelElement = context.GetData(DataConstants.PROJECT_MODEL_ELEMENT);

            var projectItem = projectModelElement as IProjectItem;
            if (projectItem == null)
                return null;

            if (!projectItem.Location.Directory.ExistsDirectory)
                return null;

            return projectItem.Location;
        }
    }
}