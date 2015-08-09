using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace NUnit.That.Resharper_v9.Plugin
{
    public static class ExtensionMethods
    {
        [CanBeNull]
        public static IAttribute GetAttributeExact(this IAttributesOwnerDeclaration self, List<string> attributesList)
        {
            if (self == null || !self.Attributes.Any())
                return null;

            return self.Attributes.Where(a =>
            {
                var valid = a.IsValid();
                if (!valid)
                    return false;

                return attributesList.Exists(s => s == a.Name.QualifiedName);
                //                return a.Name.QualifiedName == attributeFQN;
                //
                //                var resolved = a.Name.Reference.Resolve();
                //                if (resolved.IsValid())
                //                {
                //                    var ic = resolved.Result.DeclaredElement as ITypeElement;
                //                    if (ic != null)
                //                    {
                //                        if (ic.GetClrName().FullName.Equals(attributeFQN, StringComparison.InvariantCultureIgnoreCase))
                //                            return true;
                //                    }
                //                }
                //                return false;
            }).FirstOrDefault();
        }
    }
}