$dotNetVersion = "4.0"
$regKey = "HKLM:\software\Microsoft\MSBuild\ToolsVersions\$dotNetVersion"
$regProperty = "MSBuildToolsPath"

$msbuildExe = join-path -path (Get-ItemProperty $regKey).$regProperty -childpath "msbuild.exe"

&$msbuildExe CatBrows.Generator\CatBrows.Generator.csproj


Copy-Item -Force -Path CatBrows.Generator\bin\Debug\*.* -Destination Tests\plugin
#Remove-Item -Path Tests\*.feature.cs
#echo "" > .\Tests\BrowserTest.feature.cs
&packages\SpecFlow.1.9.0\tools\specflow.exe generateall Tests\CatBrows.Generator.Tests.csproj
&$msbuildExe Tests\CatBrows.Generator.Tests.csproj¨&.\packages\NUnit.Runners.2.6.2\tools\nunit-console.exe Tests\bin\Debug\CatBrows.Generator.Tests.dll