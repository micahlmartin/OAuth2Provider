param (
	[System.Collections.Hashtable]$properties = @{}
)

$root = Split-Path -parent $MyInvocation.MyCommand.Definition

Import-Module $root\tools\psake\psake.psm1 -ErrorAction SilentlyContinue

Invoke-psake $root\default.ps1 -properties $properties

Remove-Module psake -ErrorAction SilentlyContinue