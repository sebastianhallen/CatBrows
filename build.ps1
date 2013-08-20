$dotNetVersion = "4.0"
$regKey = "HKLM:\software\Microsoft\MSBuild\ToolsVersions\$dotNetVersion"
$regProperty = "MSBuildToolsPath"

$msbuildExe = join-path -path (Get-ItemProperty $regKey).$regProperty -childpath "msbuild.exe"

&$msbuildExe 


Copy-Item -Force -Path BrowserTestGenerator\bin\Debug\*.* -Destination Tests\plugin
packages\SpecFlow.1.9.0\tools\specflow.exe generateall Tests\Tests.csproj