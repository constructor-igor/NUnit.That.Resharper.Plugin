# NUnit.That.Resharper.Plugin
[Resharper(https://www.jetbrains.com/resharper/)] plugin for transfer to Assert.That (include NUnit v3 support).

NUnit.That.Resharper.Plugin helps to convert Assert methods and nunit attributes to Assert.That method.

[Resharper gallery(https://resharper-plugins.jetbrains.com/packages/NUnit.That.Resharper_v8.Plugin/)]

## Short List:

 - Attribute ExpectedException to Assert.That construction

 - Assert.IsNullOrEmpty(expression) to Assert.That(expression, Is.Null.Or.Empty)
 - [next] Assert.IsNotNullOrEmpty(expression) to Assert.That(expression, Is.Not.Null.Or.Empty)

## Samples:
