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
using JetBrains.VsIntegration.ProjectDocuments;
using JetBrains.VsIntegration.ProjectDocuments.Projects.Builder;
using NUnit.That.Resharper_v9.Plugin.TabManager;
using VSLangProj;

namespace NUnit.That.Resharper_v9.Plugin.ProjectActions
{
    [Action("NUnit.That.Resharper_v9.Plugin.ProjectActions.ProjectReferencesSetCopyLocalFalseActionItem", "Project: Set 'Copy Local == False' references in 'Output'", Id = 20206)]
    public class ProjectReferencesSetCopyLocalFalseActionItem : IExecutableAction, IInsertLast<IntoSolutionItemGroup_Modify_Project>
    {
        #region Implementation of IExecutableAction
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            int count = GetReferencesList(context).Count(r=>r.CopyLocal);
            if (count > 0)
                presentation.Text = String.Format("Project: Set 'Copy Local == False' references ({0}) in 'Output'", count);
            return count > 0;
        }
        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {            
            List<IProjectToModuleReference> allReferences = GetReferencesList(context);
            List<IProjectToModuleReference> copyLocalFalseReferencesTrue = allReferences.Where(r => r.CopyLocal).ToList();
            if (copyLocalFalseReferencesTrue.Count > 0)
            {
                ITabManager outputTabManager = new OutputTabManager()
                    .OutputHeader()
                    .OutputString("List of 'Copy Local' == True references ({0}):\n", copyLocalFalseReferencesTrue.Count, allReferences.Count);
                copyLocalFalseReferencesTrue
                    .ForEach(r => outputTabManager.OutputString("\t{0}\n", r.Name));
                outputTabManager.OutputString("\n");


                ITabManager debugTabManager = DebugTabManager.Create();
                
                IProject project = context.Projects().Project;

                debugTabManager.OutputLine("project.Name:{0}", project.Name);

                ProjectModelSynchronizer synchronizer = context.GetComponent<ProjectModelSynchronizer>();

                debugTabManager.OutputLine("synchronizer, totalProjects = {0}", synchronizer.Statistics.TotalProjects);

                VSProjectInfo vsProjectInfo = synchronizer.GetProjectInfoByProject(project);

                debugTabManager.OutputLine("vsProjectInfo = {0}", vsProjectInfo.Project.Name);

                EnvDTE.Project extProject = vsProjectInfo.GetExtProject();
                if (extProject != null)
                {
                    VSLangProj.VSProject vsProject = (VSLangProj.VSProject)extProject.Object;

                    debugTabManager.OutputLine("vsProject = {0}", vsProject.Project.Name);
                    debugTabManager.OutputLine("vsProject.References.Count = {0}", vsProject.References.Count);

                    for (int i = 0; i < vsProject.References.Count; i++)
                    {
                        try
                        {
                            Reference reference = vsProject.References.Item(i);
                            debugTabManager.OutputLine("reference: Identity = {0}, Name = {1}, version={2}",
                                reference.Identity, reference.Name, reference.Version);
                            reference.CopyLocal = false;
                        }
                        catch (Exception e)
                        {
                            debugTabManager.OutputLine("exception:", e.Message);
                        }
                    }
                }
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