using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace NUnit.That.Resharper_v8.Plugin
{
    public static class ExtensionMethods
    {
        [CanBeNull]
        public static IAttribute GetAttributeExact(this IAttributesOwnerDeclaration self, string attributeFQN)
        {
            return self.Attributes.Where(a =>
            {
                var valid = a.IsValid();
                if (!valid)
                    return false;
                var resolved = a.Name.Reference.Resolve();
                if (resolved.IsValid())
                {
                    var ic = resolved.Result.DeclaredElement as ITypeElement;
                    if (ic != null)
                    {
                        if (ic.GetClrName().FullName.Equals(attributeFQN, StringComparison.InvariantCultureIgnoreCase))
                            return true;
                    }
                }
                return false;
            }).FirstOrDefault();
        }
    }
}