$dotNetVersion = "4.0"
$regKey = "HKLM:\software\Microsoft\MSBuild\ToolsVersions\$dotNetVersion"
$regProperty = "MSBuildToolsPath"

$msbuildExe = join-path -path (Get-ItemProperty $regKey).$regProperty -childpath "msbuild.exe"
$specflowFolder = ".\packages\SpecFlow.1.9.0\tools"
$specflowExe = $specflowFolder + "\specflow.exe"
$nunitExe = ".\packages\NUnit.Runners.2.6.2\tools\nunit-console.exe"



&$msbuildExe CatBrows.Generator\CatBrows.Generator.csproj
Copy-Item -Force -Path CatBrows.Generator\bin\Debug\CatBrows.Generator.SpecFlowPlugin.dll -Destination Tests\plugin

Write-Host "###################################"
Write-Host "regenerating specflow files"
Copy-Item -Force -Path .\packages\specflow.exe.config -Destination $specflowFolder
&$specflowExe generateall TestSample.BrowserGuardDisabled\TestSample.BrowserGuardDisabled.csproj /force /verbose&$specflowExe generateall TestSample.BrowserGuardEnabled\TestSample.BrowserGuardEnabled.csproj /force /verbose&$specflowExe generateall TestSample.DefaultSettings\TestSample.DefaultSettings.csproj /force /verbose
Write-Host ""


Write-Host "###################################"
Write-Host "rebuilding test projects with regenerated spec flow files"
&$msbuildExe /t:Build Tests\CatBrows.Generator.Tests.csproj&$msbuildExe /t:Build TestSample.BrowserGuardDisabled\TestSample.BrowserGuardDisabled.csproj&$msbuildExe /t:Build TestSample.BrowserGuardEnabled\TestSample.BrowserGuardEnabled.csproj&$msbuildExe /t:Build TestSample.DefaultSettings\TestSample.DefaultSettings.csprojWrite-Host ""
&$nunitExe Tests\bin\Debug\CatBrows.Generator.Tests.dll TestSample.BrowserGuardDisabled\bin\Debug\TestSample.BrowserGuardDisabled.dll TestSample.BrowserGuardEnabled\bin\Debug\TestSample.BrowserGuardEnabled.dll TestSample.DefaultSettings\bin\Debug\TestSample.DefaultSettings.dll