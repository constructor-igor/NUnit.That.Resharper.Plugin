using System;
using System.Diagnostics;
using JetBrains.ActionManagement;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Actions;
using JetBrains.UI.ActionsRevised;
using JetBrains.Util;

namespace NUnit.That.Resharper_v9.Plugin.ProjectActions
{
    /*
     * 
     It would look something like:

    [Action("MyActionId", "Action text on menu" Icon = typeof(MyIcons.MyAction)]
    public class MyAction : IExecutableAction, IInsertLast<IntoSolutionItemGroup_Misc>
    {
        // Implementation of IExecutableAction
        ...
    }

    This would create an action with an ID of "MyActionId", displaying "Action text on menu" on the menu, and inserting itself into the project + solution context menu, as the last item of the "misc" group. The misc group is the group that includes things such as "Collapse All". It also won't necessarily be last, as there could be multiple actions using IInsertLast - it will get added to the end of the menu, but other items may then be added to the end of the menu afterwards.

    Regards
    Matt
     * */
    [Action("NUnit.That.Resharper_v9.Plugin.ProjectActions.ProjectOutputInExplorerActionItem", "Open Output Folder in File Explorer", Id = 20202)]
    public class ProjectOutputInExplorerActionItem : IExecutableAction, IInsertLast<IntoSolutionItemGroup_Modify_Project>
    {
        #region Implementation of IExecutableAction
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            FileSystemPath outputFolder = GetPathByContext(context);
            bool visible = outputFolder != null && !outputFolder.IsNullOrEmpty();
            return visible;
        }
        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            FileSystemPath path = GetPathByContext(context);
            if (path == null)
                return;

            ProcessStartInfo processStartInfo = GetProcessStartInfo(path);

            try
            {
                using (Process.Start(processStartInfo)) { }
            }
            catch (Exception e)
            {
                MessageBox.ShowError(e.Message);
            }
        }
        #endregion

        [NotNull]
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