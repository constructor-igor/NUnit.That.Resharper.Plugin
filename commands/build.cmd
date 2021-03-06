@echo off
set config=%1
if "%config%" == "" (
   set config=Debug
)
set nuget=nuget\.nuget\NuGet.exe
set version=0.10.0.6
set outputDirectory=..\Packages\%config%

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild ..\sources\NUnit.That.Resharper.Plugin.sln /t:Rebuild /p:Configuration="%config%" /m /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false 

rem
rem removed "-Symbols" from "nuget pack" 
rem 

%nuget% pack "..\sources\NUnit.That.Resharper_v10.Plugin\NUnit.That.Resharper_v10.Plugin.nuspec" -Version %version% -OutputDirectory %outputDirectory% -Prop Configuration="%config%" -NoPackageAnalysis

if "%config%" == "Release" (
	%nuget% push %outputDirectory%\NUnit.That.Resharper_v10.Plugin.%version%.nupkg -Source https://resharper-plugins.jetbrains.com
)

@echo on
