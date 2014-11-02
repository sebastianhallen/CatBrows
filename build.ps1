Function BuildGenerator($buildConfiguration) 
{
	$msbuildExe = GetMsbuildExePath
    $cleanArg = '/t:Clean'

    Write-Host 'Cleaning solution...'
	$clean = &$msbuildExe .\src\CatBrows.sln $cleanArg
	if ($LastExitCode -ne 0) {
		throw "Clean failed: $clean"
	}

    $buildArg = '/t:Build'
    $configurationArg = "/p:Configuration=$buildConfiguration"

    Write-Host "Building generator with configuration $buildConfiguration"
    $build = &$msbuildExe .\src\CatBrows.Generator\CatBrows.Generator.csproj $buildArg $configurationArg
	if ($LastExitCode -ne 0) {
		throw "Build failed: $build"
	}
	
	$pluginPath = "src\NuGetPackage\plugin\CatBrows.Generator.SpecFlowPlugin.dll"
	Write-Host "Plugin path: $pluginPath"
	
	return $pluginPath
}


Function GenerateFeatures($generatorDllPath)
{
	$specflowFolder = '.\src\packages\SpecFlow.1.9.0\tools'
	$specflowExe = "$specflowFolder\specflow.exe"
    $msbuildExe = GetMsbuildExePath

	Write-Host $generatorDllPath
	if (!(Test-Path -path $generatorDllPath)) {
		throw "Unable to find $generatorDllPath"
	}

	Copy-Item -Force -Path $generatorDllPath -Destination src\Tests\plugin

	Write-Host "###################################"
	Write-Host "regenerating specflow files"
	Copy-Item -Force -Path .\src\packages\specflow.exe.config -Destination $specflowFolder
    $gen = &$specflowExe generateall src\TestSample.BrowserGuardDisabled\TestSample.BrowserGuardDisabled.csproj /force /verbose
    if ($LastExitCode -ne 0) {
		throw "unable to generate nunit code: $gen"
	}
	$gen = &$specflowExe generateall src\TestSample.BrowserGuardEnabled\TestSample.BrowserGuardEnabled.csproj /force /verbose
	if ($LastExitCode -ne 0) {
		throw "unable to generate nunit code: $gen"
	}

    $gen = &$specflowExe generateall src\TestSample.DefaultSettings\TestSample.DefaultSettings.csproj /force /verbose
    if ($LastExitCode -ne 0) {
		throw "unable to generate nunit code: $gen"
	}

	Write-Host ""
	Write-Host "###################################"
	Write-Host "rebuilding test projects with regenerated spec flow files"
    $buildArg = "/t:Build"
	$build = &$msbuildExe $buildArg src\Tests\CatBrows.Generator.Tests.csproj
	if ($LastExitCode -ne 0) {
		throw "failed to build generated code: $build"
	}
    $build = &$msbuildExe $buildArg src\TestSample.BrowserGuardDisabled\TestSample.BrowserGuardDisabled.csproj
	if ($LastExitCode -ne 0) {
		throw "failed to build generated code: $build"
	}
    $build = &$msbuildExe $buildArg src\TestSample.BrowserGuardEnabled\TestSample.BrowserGuardEnabled.csproj
	if ($LastExitCode -ne 0) {
		throw "failed to build generated code: $build"
	}
    $build = &$msbuildExe $buildArg src\TestSample.DefaultSettings\TestSample.DefaultSettings.csproj
	if ($LastExitCode -ne 0) {
		throw "failed to build generated code: $build"
	}
    Write-Host ""
}

Function BuildSolution($buildConfiguration)
{
    $msbuildExe = GetMsbuildExePath
    $buildArg = '/t:Build'
    $configurationArg = "/p:Configuration=$buildConfiguration"

    Write-Host "Building solution with configuration $buildConfiguration"
    $build = &$msbuildExe .\src\CatBrows.sln $buildArg $configurationArg
	if ($LastExitCode -ne 0) {
		throw "Build failed: $build"
	}
}

Function RunTests($buildConfiguration)
{
	$nunitExe = ".\src\packages\NUnit.Runners.2.6.2\tools\nunit-console.exe"
	$testDlls = @("src\Tests\bin\$buildConfiguration\CatBrows.Generator.Tests.dll", "src\TestSample.BrowserGuardDisabled\bin\$buildConfiguration\TestSample.BrowserGuardDisabled.dll", "src\TestSample.BrowserGuardEnabled\bin\$buildConfiguration\TestSample.BrowserGuardEnabled.dll", "src\TestSample.DefaultSettings\bin\$buildConfiguration\TestSample.DefaultSettings.dll")
	
	&$nunitExe $testDlls
	if ($LastExitCode -ne 0) {
		throw "$LastExitCode Tests failed"
	}

    Write-Host "Tests passed"
}

Function GetMsbuildExePath()
{
	$dotNetVersion = "4.0"
	$regKey = "HKLM:\software\Microsoft\MSBuild\ToolsVersions\$dotNetVersion"
	$regProperty = "MSBuildToolsPath"

	$msbuildExe = join-path -path (Get-ItemProperty $regKey).$regProperty -childpath "msbuild.exe"

	return $msbuildExe
}

Function Run($buildConfiguration = "Release")
{
	$generatorDllPath = BuildGenerator($buildConfiguration)
	GenerateFeatures($generatorDllPath)
	BuildSolution($buildConfiguration)
	RunTests($buildConfiguration)
}

Run