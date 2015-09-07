using System.Collections.Generic;
using System.Linq;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ReSharper.Feature.Services.Actions;
using JetBrains.UI.ActionsRevised;

namespace NUnit.That.Resharper_v9.Plugin
{
    [Action("NUnit.That.Resharper_v9.Plugin.ProjectReferencesCopyLocalFalseSolutionActionItem", "Solution: Show 'Copy Local == True' references in 'Output'", Id = 20204)]
    public class ProjectReferencesCopyLocalFalseSolutionActionItem : IExecutableAction, IInsertLast<IntoSolutionItemGroup_Misc>
    {
        #region Implementation of IExecutableAction
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            return context.Projects().Solution!=null;
        }
        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            ISolution solution = context.Projects().Solution;
            ICollection<IProject> allProjects = solution.GetAllProjects();

            OutputTabManager outputTabManager = new OutputTabManager();
            foreach (IProject project in allProjects)
            {
                if (project != null)
                {
                    List<IProjectToModuleReference> allReferences = project.GetModuleReferences(TargetFrameworkId.Default).ToList();
                    if (allReferences.Any())
                    {                        
                        List<IProjectToModuleReference> copyLocalFalseReferencesList = allReferences.Where(r => r.CopyLocal).ToList();
                        if (copyLocalFalseReferencesList.Count > 0)
                        {
                            outputTabManager.OutputString("\nProject: {0}", project.Name);
                            outputTabManager.OutputString("\nList of 'Copy Local' == True references ({0}):\n", copyLocalFalseReferencesList.Count, allReferences.Count);
                            copyLocalFalseReferencesList.ForEach(r => outputTabManager.OutputString("\t{0}\n", r.Name));
                            outputTabManager.OutputString("\n");
                        }
                    }
                }
            }
        }
        #endregion
    }
}