# NUnit.That.Resharper.Plugin
[Resharper](https://www.jetbrains.com/resharper/) plugin for transfer to Assert.That (include NUnit v3 support).

NUnit.That.Resharper.Plugin helps to convert Assert methods and nunit attributes to Assert.That method.

- NUnit.That.Resharper.Plugin in [Resharper gallery v9](https://resharper-plugins.jetbrains.com/packages/NUnit.That.Resharper_v9.Plugin/)
- NUnit.That.Resharper.Plugin in [Resharper gallery v8](https://resharper-plugins.jetbrains.com/packages/NUnit.That.Resharper_v8.Plugin/)

### Intro
Attribute "ExpectedException"

![alt tag](screens/AttributeExpectedException.png)

can be converted to relevant Assert.That

![alt tag](screens/AttributeConvertedToAssertThat.png)

### Short List:

###### Attributes
- Attribute **ExpectedException** to **Assert.That(code, Throws.TypeOf<Exception>())** construction
- Attribute **Ignore** without parameter to **Ignore("")**

###### Assert methods
- Assert.**IsNotNull**(expression) to **Assert.That(expression, Is.Not.Null)**
- Assert.**IsNullOrEmpty**(expression) to **Assert.That(expression, Is.Null.Or.Empty)**
- Assert.**IsNotNullOrEmpty**(expression) to **Assert.That(expression, Is.Not.Null.Or.Empty)**

### Samples:

###### Attribute ExpectedException

Attribute ExpectedException
```c#
        [Test, ExpectedException]
        public void TestAndShortExpectedException()
        {
            foo1();
            foo2();
            foo1();
        }
```
to Assert.That(..., Throws.Exception)
```c#
        [Test]
        public void TestAndShortExpectedException()
        {
            foo1();
            Assert.That(() => { foo2(); }, Throws.Exception);
            foo1();
        }
```
Atribute ExpectedException with concrete exception type
```c#
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestExpectedException()
        {
            foo1();
            foo4();
            foo1();
        }
```
to Assert.That(..., Throws.TypeOf())
```c#
        [Test]
        public void TestExpectedException()
        {
            foo1();
            Assert.That(() => { foo4(); }, Throws.TypeOf<ArgumentException>());
            foo1();
        }
```
Atribute ExpectedException with concrete exception type and expected message
```c#
        [Test]
        [ExpectedException(typeof(NotImplementedException), ExpectedMessage = "customer message")]
        public void TestExpectedExceptionWithCustomerMessage()
        {
            foo4("customer message");
        }
```
to Assert.That(..., Throws.TypeOf().And.Message.EqualTo(expected message));
```c#
        [Test]
        public void TestExpectedExceptionWithCustomerMessage()
        {
            Assert.That(() => { foo4("customer message"); }, Throws.TypeOf<NotImplementedException>().And.Message.EqualTo("customer message"));
        }
```

###### Assert.Is... methods

| Assert.Is...  | Assert.That |
| ------------- | ------------- |
| ```Assert.IsNotNull(actual);``` | ```Assert.That(actual, Is.Not.Null);``` |
| ```Assert.IsNullOrEmpty(actual);``` | ```Assert.That(actual, Is.Null.Or.Empty);``` |
| ```Assert.IsNotNullOrEmpty(actual);```  | ```Assert.That(actual, Is.Not.Null.Or.Empty);``` |
