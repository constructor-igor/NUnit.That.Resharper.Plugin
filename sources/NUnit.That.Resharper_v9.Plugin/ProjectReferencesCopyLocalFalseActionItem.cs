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
    [Action("NUnit.That.Resharper_v9.Plugin.ProjectReferencesCopyLocalFalseActionItem", "Show 'Copy Local == False' references in 'Output'", Id = 20203)]
    public class ProjectReferencesCopyLocalFalseActionItem : IExecutableAction, IInsertLast<IntoSolutionItemGroup_Modify_Project>
    {
        #region Implementation of IExecutableAction
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            int count = GetReferencesList(context).Count(r=>r.CopyLocal);
            if (count > 0)
                presentation.Text = String.Format("Show 'Copy Local == True' references ({0}) in 'Output'", count);
            return count > 0;
        }
        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {            
//            GetReferencesList(context)
//                .ForEach(r=>r.SetProperty(new Key("Copy Local"), false));            
            List<IProjectToModuleReference> allReferences = GetReferencesList(context);
            List<IProjectToModuleReference> copyLocalFalseReferencesList = allReferences.Where(r => r.CopyLocal).ToList();
            if (copyLocalFalseReferencesList.Count > 0)
            {
                OutputTabManager outputTabManager = new OutputTabManager();
                outputTabManager.OutputString("\nList of 'Copy Local' == True references ({0}) from {1}:\n", copyLocalFalseReferencesList.Count, allReferences.Count());
                copyLocalFalseReferencesList
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