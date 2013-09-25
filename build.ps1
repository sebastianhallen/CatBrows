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
&$specflowExe generateall Tests\CatBrows.Generator.Tests.csproj /force /verbose
Write-Host ""


Write-Host "###################################"
Write-Host "rebuilding test project with regenerated spec flow files"
&$msbuildExe /t:Rebuild Tests\CatBrows.Generator.Tests.csprojWrite-Host ""
&$nunitExe Tests\bin\Debug\CatBrows.Generator.Tests.dll