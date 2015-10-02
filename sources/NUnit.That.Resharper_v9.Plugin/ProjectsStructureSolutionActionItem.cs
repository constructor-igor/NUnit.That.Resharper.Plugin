using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ActionManagement;
using JetBrains.Application.DataContext;
using JetBrains.Metadata.Reader.API;
using JetBrains.ProjectModel;
using JetBrains.ProjectModel.DataContext;
using JetBrains.ProjectModel.Model2.References;
using JetBrains.ReSharper.Feature.Services.Actions;
using JetBrains.UI.ActionsRevised;
using JetBrains.Util;
using NUnit.That.Resharper_v9.Plugin.TabManager;

namespace NUnit.That.Resharper_v9.Plugin
{
    [Action("NUnit.That.Resharper_v9.Plugin.ProjectsStructureSolutionActionItem", "Solution: Show projects structure in 'Output'", Id = 20205)]
    public class ProjectsStructureSolutionActionItem : IExecutableAction, IInsertLast<IntoSolutionItemGroup_Misc>
    {
        #region Implementation of IExecutableAction
        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            return context.Projects().Solution != null;
        }
        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            ISolution solution = context.Projects().Solution;
            if (solution == null)
                return;

            ITabManager outputTabManager = new OutputTabManager()
                .OutputHeader()
                .OutputString("Solution: {0}", solution.Name);
            Dictionary<string, ModuleDetails> modulesList = new Dictionary<string, ModuleDetails>();
            ICollection<IProject> allProjects = solution.GetAllProjects();
            foreach (IProject project in allProjects)
            {
                if (project != null)
                {
                    List<IProjectToModuleReference> allReferences = project.GetModuleReferences(TargetFrameworkId.Default).ToList();
                    if (Enumerable.Any(allReferences))
                    {
                        outputTabManager
                            .OutputString("\nProject: {0}", project.Name)
                            .OutputString("; Output Directory: {0}", project.GetOutputDirectory())
                            .OutputString("; Target Framework: {0}", project.TargetFrameworkIds.Select(t=>t.Name).ToArray().Join(","))
                            .OutputString("\n References:");

                        foreach (IProjectToModuleReference moduleReference in allReferences)
                        {
                            IProjectToAssemblyReference assemblyReference = moduleReference as IProjectToAssemblyReference;
                            if (assemblyReference != null)
                            {
                                AssemblyReferenceTarget assemblyReferenceTarget = assemblyReference.ReferenceTarget;
                                Version moduleAssemblyVersion = assemblyReferenceTarget.AssemblyName.Version;
                                FileSystemPath moduleAssemblyLocation = assemblyReferenceTarget.HintLocation;

                                string moduleVersion = moduleAssemblyVersion != null ? moduleAssemblyVersion.ToString() : "";
                                string moduleLocation = moduleAssemblyLocation != null ? moduleAssemblyLocation.Directory.FullPath : "";
                                bool isValid = assemblyReference.IsValid();

                                outputTabManager.OutputString("\n  {0}: {1}; {2}; {3}", moduleReference.Name, moduleReference.CopyLocal, moduleVersion, moduleLocation);

                                if (!modulesList.ContainsKey(moduleReference.Name))
                                {
                                    modulesList.Add(moduleReference.Name, new ModuleDetails(moduleReference.Name));
                                }
                                modulesList[moduleReference.Name].VersionList.Add(moduleVersion);
                                modulesList[moduleReference.Name].DirectoryList.Add(moduleLocation);
                                if (!isValid)
                                    modulesList[moduleReference.Name].InvalidList.Add(string.Format("{0} - {1}", project.Name, assemblyReference.Name));
                            }
                        }
                    }
                }
            }

            outputTabManager.OutputString("\n\n List of modules with invalid mark:");
            modulesList.Values
                .Where(moduleDetails => moduleDetails.InvalidList.Any())
                .ToList()
                .ForEach(moduleDetails =>
                {
                    outputTabManager.OutputString("\n {0}:", moduleDetails.Name);
                    outputTabManager.OutputString(" {0}:", moduleDetails.InvalidList.ToArray().Join(";"));
                });

            outputTabManager.OutputString("\n\n List of modules with multi-versions:");
            modulesList.Values
                .Where(moduleDetails=>moduleDetails.VersionList.Count>1)
                .ToList()
                .ForEach(moduleDetails =>
                {
                    outputTabManager.OutputString("\n\n {0}:", moduleDetails.Name);
                    outputTabManager.OutputString(" {0}:", moduleDetails.VersionList.ToArray().Join(";"));
                });

            outputTabManager.OutputString("\n List of modules with multi-location:");
            modulesList.Values
                .Where(moduleDetails => moduleDetails.DirectoryList.Count>1)
                .ToList()
                .ForEach(moduleDetails =>
                {
                    outputTabManager.OutputString("\n {0}:", moduleDetails.Name);
                    outputTabManager.OutputString(" {0}:", moduleDetails.DirectoryList.ToArray().Join(";"));
                });
        }
        #endregion
    }

    public class ModuleDetails
    {
        public string Name { get; private set; }
        public HashSet<string> VersionList { get; private set; }
        public HashSet<string> DirectoryList { get; private set; }
        public List<string> InvalidList { get; private set; }

        public ModuleDetails(string name)
        {
            Name = name;
            VersionList = new HashSet<string>();
            DirectoryList = new HashSet<string>();
            InvalidList = new List<string>();
        }
    }
}