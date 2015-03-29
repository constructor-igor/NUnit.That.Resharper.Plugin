# NUnit.That.Resharper.Plugin
[Resharper](https://www.jetbrains.com/resharper/) plugin for transfer to Assert.That (include NUnit v3 support).

NUnit.That.Resharper.Plugin helps to convert Assert methods and nunit attributes to Assert.That method.

NUnit.That.Resharper.Plugin in [Resharper gallery](https://resharper-plugins.jetbrains.com/packages/NUnit.That.Resharper_v8.Plugin/)

### Intro
Attribute "ExpectedException"

![alt tag](screens/AttributeExpectedException.png)

can be converted to relevant Assert.That

![alt tag](screens/AttributeConvertedToAssertThat.png)

### Short List:

###### Attributes
- Attribute **ExpectedException** to Assert.That(code, Throws.TypeOf<Exception>()) construction

###### Assert methods
- Assert.**IsNullOrEmpty**(expression) to Assert.That(expression, Is.Null.Or.Empty)
- Assert.**IsNotNullOrEmpty**(expression) to Assert.That(expression, Is.Not.Null.Or.Empty)

### Samples:
