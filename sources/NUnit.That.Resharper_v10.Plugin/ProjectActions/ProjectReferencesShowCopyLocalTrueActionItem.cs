using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ActionManagement;
using JetBrains.Annotations;
using JetBrains.Application.DataContext;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Actions;
using JetBrains.UI.ActionsRevised;
using NUnit.That.Resharper_v10.Plugin.TabManager;

namespace NUnit.That.Resharper_v10.Plugin.ProjectActions
{
    [Action("NUnit.That.Resharper_v10.Plugin.ProjectActions.ProjectReferencesShowCopyLocalTrueActionItem", "Project: Show 'Copy Local == True' references in 'Output'", Id = 20203)]
    public class ProjectReferencesShowCopyLocalTrueActionItem : IExecutableAction, IInsertLast<IntoSolutionItemGroup_Modify_Project>
    {
        #region Implementation of IExecutableAction
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            int count = GetReferencesList(context).Count(r => r.CopyLocal);
            if (count > 0)
                presentation.Text = String.Format("Project: Show 'Copy Local == True' references ({0}) in 'Output'", count);
            return count > 0;
        }
        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            List<IProjectToModuleReference> allReferences = GetReferencesList(context);
            List<IProjectToModuleReference> copyLocalTrueReferencesList = allReferences.Where(r => r.CopyLocal).ToList();
            if (copyLocalTrueReferencesList.Count > 0)
            {
                ITabManager outputTabManager = new OutputTabManager()
                    .OutputHeader()
                    .OutputString("List of 'Copy Local' == True references ({0}):\n", copyLocalTrueReferencesList.Count, allReferences.Count);
                copyLocalTrueReferencesList
                    .ForEach(r => outputTabManager.OutputString("\t{0}\n", r.Name));
                outputTabManager.OutputString("\n");
            }
        }
        #endregion

        [NotNull]
        private List<IProjectToModuleReference> GetReferencesList([NotNull] IDataContext context)
        {
            ProjectsContext projectModelElement = context.Projects();
            if (projectModelElement.Project == null)
                return new List<IProjectToModuleReference>();
            return projectModelElement.Project.GetModuleReferences(TargetFrameworkId.Default).ToList();
        }
    }
}