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
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.UI.ActionsRevised;
using Microsoft.VisualStudio.Shell.Interop;

namespace NUnit.That.Resharper_v9.Plugin
{
    [Action("NUnit.That.Resharper_v9.Plugin.ProjectReferencesCopyLocalFalseActionItem", "Show 'Copy Local == False' references in 'Output'", Id = 20203)]
    public class ProjectReferencesCopyLocalFalseActionItem : IExecutableAction, IInsertLast<IntoSolutionItemGroup_Modify_Project>
    {
        private readonly Guid m_nUnitThatGuid = new Guid("D556397E-D292-4707-B924-5FFFD20802C7");
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
            List<IProjectToModuleReference> copyLocalFalseReferencesList = GetReferencesList(context).Where(r => r.CopyLocal).ToList();
            if (copyLocalFalseReferencesList.Count > 0)
            {
                OutputString(m_nUnitThatGuid, "\nList of 'Copy Local' == True references ({0}):\n", copyLocalFalseReferencesList.Count);
                copyLocalFalseReferencesList
                    .ForEach(r => OutputString(m_nUnitThatGuid, "\t{0}\n", r.Name));
                OutputString(m_nUnitThatGuid, "\n");
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

        // 
        // http://www.mztools.com/articles/2014/MZ2014017.aspx
        //
        private void OutputString(Guid guidPane, string text, params object[] parameters)
        {
            const int VISIBLE = 1;
            const int DO_NOT_CLEAR_WITH_SOLUTION = 0;
           
            // Get the output window
            JetBrains.Util.Lazy.Lazy<IVsOutputWindow> outputWindow = Shell.Instance.GetComponent<JetBrains.Util.Lazy.Lazy<IVsOutputWindow>>();

            int hr = outputWindow.Value.CreatePane(guidPane, "NUnit.That", VISIBLE, DO_NOT_CLEAR_WITH_SOLUTION);
            
            // Get the pane
            IVsOutputWindowPane outputWindowPane;
            hr = outputWindow.Value.GetPane(guidPane, out outputWindowPane);

            // Output the text
            if (outputWindowPane != null)
            {
                outputWindowPane.Activate();
                outputWindowPane.OutputString(String.Format(text, parameters));
            }
        }
    }
}