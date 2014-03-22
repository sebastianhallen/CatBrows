$dotNetVersion = "4.0"
$regKey = "HKLM:\software\Microsoft\MSBuild\ToolsVersions\$dotNetVersion"
$regProperty = "MSBuildToolsPath"

$msbuildExe = join-path -path (Get-ItemProperty $regKey).$regProperty -childpath "msbuild.exe"
$specflowFolder = ".\src\packages\SpecFlow.1.9.0\tools"
$specflowExe = $specflowFolder + "\specflow.exe"
$nunitExe = ".\src\packages\NUnit.Runners.2.6.2\tools\nunit-console.exe"



&$msbuildExe src\CatBrows.Generator\CatBrows.Generator.csproj
Copy-Item -Force -Path src\CatBrows.Generator\bin\Debug\CatBrows.Generator.SpecFlowPlugin.dll -Destination src\Tests\plugin

Write-Host "###################################"
Write-Host "regenerating specflow files"
Copy-Item -Force -Path .\src\packages\specflow.exe.config -Destination $specflowFolder
&$specflowExe generateall src\TestSample.BrowserGuardDisabled\TestSample.BrowserGuardDisabled.csproj /force /verbose&$specflowExe generateall src\TestSample.BrowserGuardEnabled\TestSample.BrowserGuardEnabled.csproj /force /verbose&$specflowExe generateall src\TestSample.DefaultSettings\TestSample.DefaultSettings.csproj /force /verbose
Write-Host ""


Write-Host "###################################"
Write-Host "rebuilding test projects with regenerated spec flow files"
&$msbuildExe /t:Build src\Tests\CatBrows.Generator.Tests.csproj&$msbuildExe /t:Build src\TestSample.BrowserGuardDisabled\TestSample.BrowserGuardDisabled.csproj&$msbuildExe /t:Build src\TestSample.BrowserGuardEnabled\TestSample.BrowserGuardEnabled.csproj&$msbuildExe /t:Build src\TestSample.DefaultSettings\TestSample.DefaultSettings.csprojWrite-Host ""
&$nunitExe src\Tests\bin\Debug\CatBrows.Generator.Tests.dll src\TestSample.BrowserGuardDisabled\bin\Debug\TestSample.BrowserGuardDisabled.dll src\TestSample.BrowserGuardEnabled\bin\Debug\TestSample.BrowserGuardEnabled.dll src\TestSample.DefaultSettings\bin\Debug\TestSample.DefaultSettings.dll