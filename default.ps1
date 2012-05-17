properties {
	$TargetFramework = "net-4.0"
	$DownloadDependentPackages = $true
}

$baseDir  = resolve-path .
$buildBase = "$baseDir\build"
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

include $toolsDir\psake\buildutils.ps1

task default -depends ReleaseOAuth

task Init {
	create-directory $binariesDir
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
 	$solutionFile = "$baseDir\OAuth.sln"
	exec { &$script:msBuild $solutionFile /p:OutDir="$buildBase\" }
	
	$assemblies = @()
	$assemblies  +=  dir $buildBase\*.dll -Exclude **Tests.dll

	#& $ilMergeTool $ilMergeKey $outDir "NServiceBus" $assemblies "" "dll" $script:ilmergeTargetFramework "$buildBase\NServiceBusMergeLog.txt" $ilMergeExclude
	& $ilMergeTool /lib:$baseDir /t:library /out:"$binariesDir\CrackerJack.dll" /targetplatform:$script:ilmergeTargetFramework /log:"$buildBase\MergeLog.txt" $assemblies
	#"CrackerJack.OAuth" $assemblies "" "dll" $script:ilmergeTargetFramework "$buildBase\MergeLog.txt"
	$mergeLogContent = Get-Content "$buildBase\MergeLog.txt"
	echo "------------------------------Merge Log-----------------------"
	echo $mergeLogContent
 }
 
task ReleaseOAuth -depends CompileMain {
 
 }

