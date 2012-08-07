properties {
	$ProductVersion = "1.0"
	$BuildNumber = "0";
	$PatchVersion = "2"
	$TargetFramework = "net-4.0"
	$DownloadDependentPackages = $true
	$UploadPackage = $false
	$NugetKey = ""
}

$baseDir  = resolve-path .
$releaseRoot = "$baseDir\Release"
$releaseDir = "$releaseRoot\net40"
$buildBase = "$baseDir\build"
$sourceDir = "$baseDir\src"
$samplesDir = "$baseDir\samples"
$outDir =  "$buildBase\output"
$toolsDir = "$baseDir\tools"
$binariesDir = "$baseDir\binaries"
$ilMergeTool = "$toolsDir\ILMerge\ILMerge.exe"
$nugetExec = "$toolsDir\NuGet\NuGet.exe"
$script:msBuild = ""
$script:isEnvironmentInitialized = $false
$script:ilmergeTargetFramework = ""
$script:msBuildTargetFramework = ""	
$script:packageVersion = "1.0.0"
$nunitexec = "tools\nunit\nunit-console.exe"
$script:nunitTargetFramework = "/framework=4.0";

include $toolsDir\psake\buildutils.ps1

task default -depends ReleaseOAuth

task Clean {
	delete-directory $binariesDir -ErrorAction silentlycontinue
}

task Init -depends Clean {
	create-directory $binariesDir
}

task DetectOperatingSystemArchitecture {
	$isWow64 = ((Get-WmiObject -class "Win32_Processor" -property "AddressWidth").AddressWidth -eq 64)
	if ($isWow64 -eq $true)
	{
		$script:architecture = "x64"
	}
    echo "Machine Architecture is $script:architecture"
}

task InstallDependentPackages {
	cd "$baseDir\packages"
	$files =  dir -Exclude *.config
	cd $baseDir
	$installDependentPackages = $DownloadDependentPackages;
	if($installDependentPackages -eq $false){
		$installDependentPackages = ((($files -ne $null) -and ($files.count -gt 0)) -eq $false)
	}
	if($installDependentPackages){
	 	dir -recurse -include ('packages.config') |ForEach-Object {
		$packageconfig = [io.path]::Combine($_.directory,$_.name)

		write-host $packageconfig 

		 exec{ &$nugetExec install $packageconfig -o packages } 
		}
	}
 }
 
task InitEnvironment -depends DetectOperatingSystemArchitecture {

	if($script:isEnvironmentInitialized -ne $true){
		if ($TargetFramework -eq "net-4.0"){
			$netfxInstallroot ="" 
			$netfxInstallroot =	Get-RegistryValue 'HKLM:\SOFTWARE\Microsoft\.NETFramework\' 'InstallRoot' 
			
			$netfxCurrent = $netfxInstallroot + "v4.0.30319"
			
			$script:msBuild = $netfxCurrent + "\msbuild.exe"
			
			echo ".Net 4.0 build requested - $script:msBuild" 

			$programFilesPath = (gc env:ProgramFiles)
			if($script:architecture -eq "x64") {
				$programFilesPath = (gc env:"ProgramFiles(x86)")
			}
			
			$frameworkPath = Join-Path $programFilesPath "Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0"
			$script:ilmergeTargetFramework  =  "v4,$frameworkPath"
			
			$script:msBuildTargetFramework ="/p:TargetFrameworkVersion=v4.0 /ToolsVersion:4.0"
			
			$script:nunitTargetFramework = "/framework=4.0";
			
			$script:isEnvironmentInitialized = $true
		}
	
	}
}
 
task CompileMain -depends InstallDependentPackages, InitEnvironment, Init {
 	$solutionFile = "$sourceDir\OAuth.sln"
	exec { &$script:msBuild $solutionFile /p:OutDir="$buildBase\" }
	
	$assemblies = @()
	$assemblies  +=  dir $buildBase\OAuth2Provider.dll
	$assemblies  +=  dir $buildBase\newtonsoft.json.dll
	$assemblies  +=  dir $buildBase\log4net.dll

	& $ilMergeTool /lib:$baseDir /t:library /out:"$binariesDir\OAuth2Provider.dll" /targetplatform:$script:ilmergeTargetFramework /log:"$buildBase\MergeLog.txt" $assemblies
	$mergeLogContent = Get-Content "$buildBase\MergeLog.txt"
	echo "------------------------------Merge Log-----------------------"
	echo $mergeLogContent
 }
 
 task TestMain -depends CompileMain {

	if((Test-Path -Path $buildBase\test-reports) -eq $false){
		Create-Directory $buildBase\test-reports 
	}
	$testAssemblies = @()
	$testAssemblies +=  dir $buildBase\*Tests.dll
	exec {&$nunitexec $testAssemblies $script:nunitTargetFramework}
}

task CompileSamples -depends InstallDependentPackages, InitEnvironment, Init {
 	$solutionFile = "$samplesDir\Samples.sln"
	exec { &$script:msBuild $solutionFile }
 }

task PrepareRelease -depends CompileMain, TestMain {
	
	if((Test-Path $releaseRoot) -eq $true){
		Delete-Directory $releaseRoot	
	}
	
	Create-Directory $releaseRoot
	if ($TargetFramework -eq "net-4.0"){
		$releaseDir = "$releaseRoot\net40"
	}
	Create-Directory $releaseDir
	
	Copy-Item -Force -Recurse "$baseDir\binaries" $releaseDir\binaries -ErrorAction SilentlyContinue  
}
 
task CreatePackages -depends PrepareRelease  {

	if(($UploadPackage) -and ($NugetKey -eq "")){
		throw "Could not find the NuGet access key Package Cannot be uploaded without access key"
	}
		
	import-module $toolsDir\NuGet\packit.psm1
	Write-Output "Loading the module for packing.............."
	$packit.push_to_nuget = $UploadPackage 
	$packit.nugetKey  = $NugetKey
	
	$packit.framework_Isolated_Binaries_Loc = "$baseDir\release"
	$packit.PackagingArtifactsRoot = "$baseDir\release\PackagingArtifacts"
	$packit.packageOutPutDir = "$baseDir\release\packages"
	$packit.targeted_Frameworks = "net40";

	$packageName = "OAuth2Provider"
	$packit.package_description = "OAuth 2 Provider"
	invoke-packit $packageName $script:packageVersion @{} "binaries\OAuth2Provider.dll" @{} 
		
	remove-module packit
 } 
 
 task GenerateAssemblyInfo {
	if($env:BUILD_NUMBER -ne $null) {
    	$BuildNumber = $env:BUILD_NUMBER
	}
	
	Write-Output "Build Number: $BuildNumber"
	
	$asmVersion = $ProductVersion + "." + $PatchVersion + "." + $BuildNumber 
	$script:packageVersion = $asmVersion
		
	Write-Output "##teamcity[buildNumber '$asmVersion']"
	
		$asmInfo = "using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Runtime.CompilerServices;

[assembly: AssemblyCompany(""Micah Martin"")]
[assembly: AssemblyFileVersion(""$asmVersion"")]
[assembly: AssemblyVersion(""$asmVersion"")]
[assembly: ComVisible(false)]		
"

	sc -Path "$baseDir\SharedAssemblyInfo.cs" -Value $asmInfo
}

task ReleaseOAuth -depends GenerateAssemblyInfo, CreatePackages, CompileSamples {
 
 }

