# NUnit.That.Resharper.Plugin
Resharper plugin for NUnit support

Convert nunit v3 not supported constructions (Assert, attributes) to relevant v3 style.

List:
 - Assert.IsNullOrEmpty(expession) to Assert.That(experession, Is.Null.Or.Empty)
 - [next] Assert.IsNotNullOrEmpty(expession) to Assert.That(experession, Is.Not.Null.Or.Empty)
