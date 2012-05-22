param (
	[System.Collections.Hashtable]$properties = @{}
)

Import-Module .\tools\psake\psake.psm1 -ErrorAction SilentlyContinue

Invoke-psake .\default.ps1 -properties $properties

Remove-Module psake -ErrorAction SilentlyContinue