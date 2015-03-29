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

###### Attribute ExpectedException

```c#
        [Test, ExpectedException]
        public void TestAndShortExpectedException()
        {
            foo1();
            foo2();
            foo1();
        }
```
to
```c#
        [Test]
        public void TestAndShortExpectedException()
        {
            foo1();
            Assert.That(() => { foo2(); }, Throws.Exception);
            foo1();
        }
```
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
to
```c#
        [Test]
        public void TestExpectedException()
        {
            foo1();
            Assert.That(() => { foo4(); }, Throws.TypeOf<ArgumentException>());
            foo1();
        }
```

```c#
        [Test]
        [ExpectedException(typeof(NotImplementedException), ExpectedMessage = "customer message")]
        public void TestExpectedExceptionWithCustomerMessage()
        {
            foo4("customer message");
        }
```
to 
```c#
        [Test]
        public void TestExpectedExceptionWithCustomerMessage()
        {
            Assert.That(() => { foo4("customer message"); }, Throws.TypeOf<NotImplementedException>().And.Message.EqualTo("customer message"));
        }
```
