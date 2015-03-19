using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace NUnit.That.Resharper_v8.Plugin
{
    [ContextAction(Group = "C#",
        Name = "Replace ExpectedException attribute with constraint-based syntax.",
        Description =
            "Replace ExpectedException attribute with constraint-based syntax; e.g., changes [ExpectedException] to Assert.That(<expression>, Throws.InstanceOf<Exception>())",
        Priority = 15)]
    public class ExpectedExceptionToConstrainBased : BulbActionBase, IContextAction
    {
        private readonly ICSharpContextActionDataProvider m_provider;
        public ExpectedExceptionToConstrainBased(ICSharpContextActionDataProvider provider)
        {
            m_provider = provider;
        }

        #region BulbActionBase
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            var statement = m_provider.GetSelectedElement<IStatement>(false, false);
            if (statement != null)
            {
                StringBuilder statementText = new StringBuilder();
                statement.GetText(statementText);
                statementText.Remove(statementText.Length - 1, 1);      // TODO remove last ';'

                const string NEW_STATEMENT_FORMAT = "Assert.That(()=>$0, Throws.InstanceOf<Exception>());";
                object[] newStatementExpression = { statementText.ToString() };
                ICSharpStatement newStatement = m_provider.ElementFactory.CreateStatement(NEW_STATEMENT_FORMAT, newStatementExpression);
                statement.ReplaceBy(newStatement);

                IMethodDeclaration methodDeclaration = m_provider.GetSelectedElement<IMethodDeclaration>(false, false);
                if (methodDeclaration != null)
                {
                    IAttribute foundAttribute = methodDeclaration.GetAttributeExact("NUnit.Framework.ExpectedExceptionAttribute");
                    if (foundAttribute != null)
                        methodDeclaration.RemoveAttribute(foundAttribute);
                }
            }

            return null;
        }
        public override string Text
        {
            get { return "Replace with Assert.That"; }
        }
        #endregion

        #region #region IContextAction
        public IEnumerable<IntentionAction> CreateBulbItems()
        {
            return this.ToContextAction();
        }
        public bool IsAvailable(IUserDataHolder cache)
        {
            IMethodDeclaration methodDeclaration = m_provider.GetSelectedElement<IMethodDeclaration>(false, false);
            bool expectedExceptionDefined = methodDeclaration.GetAttributeExact("NUnit.Framework.ExpectedExceptionAttribute") != null;
            var statement = m_provider.GetSelectedElement<IStatement>(false, false);
            bool statementSelected = statement != null;
            return expectedExceptionDefined && statementSelected;
        }
        #endregion
    }
}