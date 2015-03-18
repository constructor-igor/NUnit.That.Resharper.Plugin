using System;
using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;

namespace NUnit.That.Resharper.Plugin
{
    [ContextAction(Group = "C#",
        Name = "Replace old-style Assert methods with constraint-based syntax.",
        Description = "Replace old-style Assert methods with constraint-based syntax; e.g., changes Assert.IsNullOrEmpty(MyClass.MyMethod()) to Assert.That(MyClass.MyMethod(), Is.Null.Or.Empty)",
        Priority = 15)]
    public class AssertIsNullOrEmptyToConstrainBased : BulbActionBase, IContextAction
    {
        private readonly ICSharpContextActionDataProvider m_provider;
        public AssertIsNullOrEmptyToConstrainBased(ICSharpContextActionDataProvider provider)
        {
            m_provider = provider;
        }
        #region BulbActionBase
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, JetBrains.Application.Progress.IProgressIndicator progress)
        {
            var expression = m_provider.GetSelectedElement<IInvocationExpression>(false, false);

            if (expression != null)
            {
                IList<ICSharpArgument> args = expression.Arguments;

                //                var sb = new StringBuilder("Assert.That(");
                //                sb.Append(args[1].GetText());
                //                sb.Append(", Is.EqualTo(");
                //                sb.Append(args[0].GetText());
                //                sb.Append("))");

                // now replace everything
                const string NEW_EXPRESSION_FORMAT = "Assert.That($0, Is.Null.Or.Empty)";
                object[] newExpressionArgs = { args[0].GetText() };

                //ICSharpExpression newExp = m_provider.ElementFactory.CreateExpression(sb.ToString(), new object[] { });
                ICSharpExpression newExp = m_provider.ElementFactory.CreateExpression(NEW_EXPRESSION_FORMAT, newExpressionArgs);
                expression.ReplaceBy(newExp);
            }

            return null;
        }
        public override string Text
        {
            get { return "Replace with Assert.That"; }
        }
        #endregion

        #region IContextAction
        public IEnumerable<IntentionAction> CreateBulbItems()
        {
            return this.ToContextAction();
        }
        public bool IsAvailable(JetBrains.Util.IUserDataHolder cache)
        {
            IInvocationExpression expression = m_provider.GetSelectedElement<IInvocationExpression>(false, false);
            if (expression != null && expression.InvokedExpression.GetText() == "Assert.IsNullOrEmpty")
            {
                if (expression.Arguments.Count == 1)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}