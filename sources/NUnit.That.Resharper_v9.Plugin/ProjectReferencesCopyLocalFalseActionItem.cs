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

namespace NUnit.That.Resharper_v9.Plugin
{
    [Action("NUnit.That.Resharper_v9.Plugin.ProjectReferencesCopyLocalFalseActionItem", "Set 'Copy Local' to False", Id = 20203)]
    public class ProjectReferencesCopyLocalFalseActionItem : IExecutableAction, IInsertLast<IntoSolutionItemGroup_Modify_Project>
    {
        #region Implementation of IExecutableAction
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            int count = GetReferencesList(context).Count(r=>r.CopyLocal);
            if (count > 0)
                presentation.Text = String.Format("Set 'Copy Local' to False in {0} references", count);
            return count > 0;
        }
        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            GetReferencesList(context)
                .ForEach(r=>r.CopyLocal = false);
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