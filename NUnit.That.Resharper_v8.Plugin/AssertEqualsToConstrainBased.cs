﻿using System;
using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;

namespace NUnit.That.Resharper_v8.Plugin
{
    /*
     * http://dev-ua.livejournal.com/774.html
     * http://www.nuget.org/packages/JetBrains.ReSharper.SDK/8.2.1158
     * */

    [ContextAction(Group = "C#",
        Name = "Replace old-style Assert methods with constraint-based syntax.",
        Description = "Replace old-style Assert methods with constraint-based syntax; e.g., changes Assert.Equal(10, MyClass.MyMethod()) to Assert.That(MyClass.MyMethod(), Is.EqualTo(10))",
        Priority = 15)]
    public class AssertEqualsToConstrainBased : BulbActionBase, IContextAction
    {
        private readonly ICSharpContextActionDataProvider m_provider;  
        public AssertEqualsToConstrainBased(ICSharpContextActionDataProvider provider)
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

                // now replace everything
                const string NEW_EXPRESSION_FORMAT = "Assert.That($0, Is.EqualTo($1))";
                object[] newExpressionArgs = {args[1].GetText(), args[0].GetText()};

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
            if (expression != null && expression.InvokedExpression.GetText() == "Assert.AreEqual")
            {
                if (expression.Arguments.Count == 2)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}