using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Intentions.Extensibility.Menu;
using JetBrains.ReSharper.Psi;
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
        private readonly List<string> m_attributesList = new List<string>
        {
            "ExpectedException", 
            "ExpectedExceptionAttribute", 
            "Framework.ExpectedException", 
            "Framework.ExpectedExceptionAttribute",
            "NUnit.Framework.ExpectedException", 
            "NUnit.Framework.ExpectedExceptionAttribute"
        }; 
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
                IAttribute foundAttribute = null;
                string expectedExceptionTypeOfExpr = null;
                string expectedExceptionType = null;
                string expectedExceptionMessage = null;

                IMethodDeclaration methodDeclaration = m_provider.GetSelectedElement<IMethodDeclaration>(false, false);
                if (methodDeclaration != null)
                {
                    foundAttribute = methodDeclaration.GetAttributeExact(m_attributesList);
                    if (foundAttribute != null)
                    {
                        foreach (ICSharpArgument argument in foundAttribute.Arguments)
                        {
                            IExpressionType exprType = argument.GetExpressionType();
                            if (exprType.ToString() == "System.Type")
                            {
                                expectedExceptionTypeOfExpr = argument.Value.GetText();
                                expectedExceptionType = argument.Value.FirstChild.NextSibling.NextSibling.GetText();
                            }
                        }
                        foreach (IPropertyAssignment propertyAssignment in foundAttribute.PropertyAssignments)
                        {
                            Trace.WriteLine("p:" + propertyAssignment.GetText());
                            switch (propertyAssignment.PropertyNameIdentifier.GetText())
                            {
                                case "ExpectedMessage":
                                    expectedExceptionMessage = propertyAssignment.Source.GetText();
                                    break;
                            }
                        }
                    }
                }

                StringBuilder statementText = new StringBuilder();
                statement.GetText(statementText);
                //statementText.Remove(statementText.Length - 1, 1);      // TODO remove last ';'

                string newExpressionFormat;
                object[] newStatementExpression;
                if (expectedExceptionTypeOfExpr == null)
                {
                    //const string NEW_STATEMENT_FORMAT = "Assert.That(()=>$0, Throws.InstanceOf<Exception>());";
                    newExpressionFormat = "Assert.That(()=>{$0}, Throws.Exception);";
                    newStatementExpression = new object[] { statementText.ToString() };
                }
                else
                {
                    //Assert.That(foo2, Throws.TypeOf(typeof(NotImplementedException)));
                    if (expectedExceptionMessage == null)
                    {
                        //newExpressionFormat = "Assert.That(()=>{$0}, Throws.TypeOf($1));";
                        //newStatementExpression = new object[] { statementText.ToString(), expectedExceptionTypeOfExpr };

                        newExpressionFormat = "Assert.That(()=>{$0}, Throws.TypeOf<$1>());";
                        newStatementExpression = new object[] { statementText.ToString(), expectedExceptionType };
                    }
                    else
                    {
                        //newExpressionFormat = "Assert.That(()=>{$0}, Throws.TypeOf($1).And.Message.EqualTo($2));";
                        //newStatementExpression = new object[] { statementText.ToString(), expectedExceptionTypeOfExpr, expectedExceptionMessage };

                        newExpressionFormat = "Assert.That(()=>{$0}, Throws.TypeOf<$1>().And.Message.EqualTo($2));";
                        newStatementExpression = new object[] { statementText.ToString(), expectedExceptionType, expectedExceptionMessage };
                    }
                }
                ICSharpStatement newStatement = m_provider.ElementFactory.CreateStatement(newExpressionFormat, newStatementExpression);
                statement.ReplaceBy(newStatement);

                if (foundAttribute != null)
                {
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
            //bool expectedExceptionDefined = methodDeclaration.GetAttributeExact("NUnit.Framework.ExpectedExceptionAttribute") != null;
            bool expectedExceptionDefined = methodDeclaration.GetAttributeExact(m_attributesList) != null;
            var statement = m_provider.GetSelectedElement<IStatement>(false, false);
            bool statementSelected = statement != null;
            return expectedExceptionDefined && statementSelected;
        }
        #endregion
    }
}