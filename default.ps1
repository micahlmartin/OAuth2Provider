properties {
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
$outDir =  "$buildBase\output"
$toolsDir = "$baseDir\tools"
$binariesDir = "$baseDir\binaries"
$ilMergeTool = "$toolsDir\ILMerge\ILMerge.exe"
$nugetExec = "$toolsDir\NuGet\NuGet.exe"
$script:msBuild = ""
$script:isEnvironmentInitialized = $false
$script:ilmergeTargetFramework = ""
$script:msBuildTargetFramework = ""	
$ilMergeKey = "$srcDir\NServiceBus.snk"
$script:packageVersion = "1.0.0"

include $toolsDir\psake\buildutils.ps1

task default -depends ReleaseOAuth

task Clean {
	delete-directory $binariesDir -ErrorAction silentlycontinue
}

task Init -depends Clean {
	create-directory $binariesDir
}

task InstallDependentPackages {
	cd "$sourceDir\packages"
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
 
task InitEnvironment{

	if($script:isEnvironmentInitialized -ne $true){
		if ($TargetFramework -eq "net-4.0"){
			$netfxInstallroot ="" 
			$netfxInstallroot =	Get-RegistryValue 'HKLM:\SOFTWARE\Microsoft\.NETFramework\' 'InstallRoot' 
			
			$netfxCurrent = $netfxInstallroot + "v4.0.30319"
			
			$script:msBuild = $netfxCurrent + "\msbuild.exe"
			
			echo ".Net 4.0 build requested - $script:msBuild" 

			$script:ilmergeTargetFramework  = "v4," + $netfxCurrent
			
			$script:msBuildTargetFramework ="/p:TargetFrameworkVersion=v4.0 /ToolsVersion:4.0"
			
			#$script:nunitTargetFramework = "/framework=4.0";
			
			$script:isEnvironmentInitialized = $true
		}
	
	}
}
 
task CompileMain -depends InstallDependentPackages, InitEnvironment, Init {
 	$solutionFile = "$sourceDir\OAuth.sln"
	exec { &$script:msBuild $solutionFile /p:OutDir="$buildBase\" }
	
	Copy-Item "$buildBase\OAuth2Provider.dll" $binariesDir
	
#	$assemblies = @()
#	$assemblies  +=  dir $buildBase\*.dll -Exclude **Tests.dll

#	& $ilMergeTool /lib:$baseDir /t:library /out:"$binariesDir\OAuth2Provider.dll" /targetplatform:$script:ilmergeTargetFramework /log:"$buildBase\MergeLog.txt" $assemblies
#	$mergeLogContent = Get-Content "$buildBase\MergeLog.txt"
#	echo "------------------------------Merge Log-----------------------"
#	echo $mergeLogContent
 }

task PrepareRelease -depends CompileMain {
	
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

	#region Packing NServiceBus
	$packageName = "OAuth2Provider"
	$packit.package_description = "OAuth 2 Provider"
	invoke-packit $packageName $script:packageVersion @{log4net="[2.0.0]"; "Newtonsoft.Json"="[4.5.5]"} "binaries\OAuth2Provider.dll" @{} 
	#endregion
		
	remove-module packit
 } 

task ReleaseOAuth -depends CreatePackages {
 
 }

