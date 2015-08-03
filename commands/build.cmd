@echo off
set config=%1
if "%config%" == "" (
   set config=Release
)
set nuget=nuget\.nuget\NuGet.exe
set version=0.9.0.5

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild ..\sources\NUnit.That.Resharper.Plugin.sln /t:Rebuild /p:Configuration="%config%" /m /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false 

%nuget% pack "..\sources\NUnit.That.Resharper_v9.Plugin\NUnit.That.Resharper_v9.Plugin.nuspec" -Version %version% -OutputDirectory ..\Packages\%config% -Prop Configuration="%config%" -Symbols -NoPackageAnalysis

@echo on
