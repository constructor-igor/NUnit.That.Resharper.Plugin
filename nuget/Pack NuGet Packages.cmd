rem
rem	https://docs.nuget.org/create/creating-and-publishing-a-package
rem 

if not exist NuGet mkdir NuGet

del /Q NuGet\*.*

rem .nuget\NuGet.exe pack ..\NUnit.That.Resharper_v8.Plugin\NUnit.That.Resharper_v8.Plugin.csproj -OutputDirectory NuGet -Prop Configuration=Release -Symbols
.nuget\NuGet.exe pack ..\NUnit.That.Resharper_v8.Plugin\NUnit.That.Resharper_v8.Plugin.nuspec -OutputDirectory NuGet -Prop Configuration=Release -Symbols

pause
