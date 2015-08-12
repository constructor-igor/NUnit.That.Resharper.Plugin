using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.ContextActions;
using JetBrains.ReSharper.Feature.Services.CSharp.Analyses.Bulbs;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Feature.Services.LinqTools;
using JetBrains.ReSharper.I18n.Services;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Cpp.Tree;
using JetBrains.ReSharper.Psi.CSharp.Impl.Tree;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;

namespace NUnit.That.Resharper_v9.Plugin
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
            bool moveCaretToUpdatedStatement = false;
            IMethodDeclaration methodDeclaration = m_provider.GetSelectedElement<IMethodDeclaration>(false, false);
            var statement = m_provider.GetSelectedElement<IStatement>(false, false);

            if (statement == null && methodDeclaration!=null)
            {
                TreeNodeCollection<ICSharpStatement> methodStatements = methodDeclaration.Body.Statements;
                statement = methodStatements.Last();
                moveCaretToUpdatedStatement = true;
            }

            if (statement != null)
            {
                IAttribute foundAttribute = null;
                string expectedExceptionTypeOfExpr = null;
                string expectedExceptionType = null;
                string expectedExceptionMessage = null;

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
                                expectedExceptionType = null;

                                ITreeNode child = argument.Value.FirstChild;
                                while (child != null && !(child is IUserTypeUsage))
                                {
                                    child = child.NextSibling;
                                }
                                if (child!=null)
                                    expectedExceptionType = child.GetText();
                            }
                        }
                        foreach (IPropertyAssignment propertyAssignment in foundAttribute.PropertyAssignments)
                        {
                            switch (propertyAssignment.PropertyNameIdentifier.GetText())
                            {
                                case "ExpectedMessage":
                                    expectedExceptionMessage = propertyAssignment.Source.GetText();
                                    break;
                                case "ExpectedException":
                                    expectedExceptionTypeOfExpr = propertyAssignment.Source.GetText();
                                    expectedExceptionType = propertyAssignment.Source.FirstChild.NextSibling.NextSibling.GetText();
                                    break;
                            }
                        }
                    }
                }

                bool declarationStatement = statement.NodeType == ElementType.DECLARATION_STATEMENT;
                bool lambda = !declarationStatement;

                StringBuilder statementText = new StringBuilder();
                statement.GetText(statementText);

                string codeFormat = "()=>{$0}";
                if (lambda)
                {
                    statementText.Remove(statementText.Length - 1, 1); // TODO remove last ';'
                    codeFormat = "()=>$0";
                }

                string newExpressionFormat;
                object[] newStatementExpression;
                if (expectedExceptionTypeOfExpr == null)
                {
                    //const string NEW_STATEMENT_FORMAT = "Assert.That(()=>$0, Throws.InstanceOf<Exception>());";
                    newExpressionFormat = "Assert.That(" + codeFormat + ", Throws.Exception);";
                    newStatementExpression = new object[] { statementText.ToString() };
                }
                else
                {
                    //Assert.That(foo2, Throws.TypeOf(typeof(NotImplementedException)));
                    if (expectedExceptionMessage == null)
                    {
                        //newExpressionFormat = "Assert.That(()=>{$0}, Throws.TypeOf($1));";
                        //newStatementExpression = new object[] { statementText.ToString(), expectedExceptionTypeOfExpr };

                        newExpressionFormat = "Assert.That(" + codeFormat + ", Throws.TypeOf<$1>());";
                        newStatementExpression = new object[] { statementText.ToString(), expectedExceptionType };
                    }
                    else
                    {
                        //newExpressionFormat = "Assert.That(()=>{$0}, Throws.TypeOf($1).And.Message.EqualTo($2));";
                        //newStatementExpression = new object[] { statementText.ToString(), expectedExceptionTypeOfExpr, expectedExceptionMessage };

                        newExpressionFormat = "Assert.That(" + codeFormat + ", Throws.TypeOf<$1>().And.Message.EqualTo($2));";
                        newStatementExpression = new object[] { statementText.ToString(), expectedExceptionType, expectedExceptionMessage };
                    }
                }
                ICSharpStatement newStatement = m_provider.ElementFactory.CreateStatement(newExpressionFormat, newStatementExpression);
                IStatement updatedStatement = statement.ReplaceBy(newStatement);

                //m_provider.TextControl.Caret.MoveTo(updatedStatement.);
                //m_provider.TextControl.Caret.MoveTo(updatedStatement.GetDocumentRange().TextRange.StartOffset, CaretVisualPlacement.DontScrollIfVisible);

                if (foundAttribute != null)
                {
                    methodDeclaration.RemoveAttribute(foundAttribute);
                }

                if (moveCaretToUpdatedStatement)
                    return textControl =>
                        textControl.Caret.MoveTo(updatedStatement.GetDocumentRange().TextRange.StartOffset,
                        CaretVisualPlacement.DontScrollIfVisible);
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

            bool expectedExceptionDefined = methodDeclaration.GetAttributeExact(m_attributesList) != null;
            if (!expectedExceptionDefined)
                return false;

            IStatement statement = m_provider.GetSelectedElement<IStatement>(false, false);
            if (SupportedStatement(statement))
                return true;

            IAttribute attributeDeclaration = m_provider.GetSelectedElement<IAttribute>(false, false);
            if (attributeDeclaration != null 
                && methodDeclaration.HasAnyBody() 
                && methodDeclaration.Body.Statements.Any()
                && SupportedStatement(methodDeclaration.Body.Statements.Last()))
            {
                return true;
            }

            return false;            
        }
        #endregion

        private bool SupportedStatement(IStatement statement)
        {
            return statement != null && !(statement is IUsingStatement);
        }
    }
}