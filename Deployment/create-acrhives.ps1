# Licensed to the end users under one or more agreements.
# Copyright (c) 2025 Junaid Atari, and contributors
# Website: https://github.com/blacksmoke26/

########################################################################
#################### POWERSHELL DEPLOYMENT SCRIPT ######################
#  This script creates archive from each platform published directory  #
########################################################################

Set-ExecutionPolicy RemoteSigned -Scope Process

[void][Reflection.Assembly]::LoadWithPartialName('System.IO.Compression.FileSystem')
[void][Reflection.Assembly]::LoadWithPartialName('System.IO.Path')

$dotNetVersion = "net9.0";
$assemblyName = "ContactApp.Wpf";
$execName = "contactapp";
$appVersion = "0.0.1";

$publishPath = Resolve-Path "$PWD/../${assemblyName}/bin/Release/${dotNetVersion}";
$distPath = Resolve-Path "$PWD/Builds";

[Hashtable]$deployment = [ordered]@{
	Portable = $true
	WinX64 = $true
	WinX86 = $true
	LinuxArm64 = $true
	LinuxX64 = $true
	OsxX64 = $true
}

function Resolve-Dist-Dir()
{
	param (
		[parameter(Mandatory = $true)]
		[string]$Path,
		[parameter(Mandatory = $true)]
		[string]$Platform)

	if ($Platform -eq "Portable")
	{
		return $Path;
	}

	return Resolve-Path "${Path}/${Platform}"
}

[Hashtable]$bins = [ordered]@{
	Portable = "publish"
	WinX64 = "win-x64"
	WinX86 = "win-x86"
	LinuxArm64 = "linux-arm64"
	LinuxX64 = "linux-x64"
	OsxX64 = "osx-x64"
}

[Hashtable]$distribution = [ordered]@{ }

if ($deployment.Portable -eq $true)
{
	$distribution.Portable = [Hashtable][ordered]@{
		ExecutableName = "${assemblyName}.exe"
		TargetExecutableName = "${execName}.exe"
		TargetDirectory = Resolve-Dist-Dir -Path $publishPath -Platform $bins.Portable
		CleanFiles = @("${assemblyName}.pdb")
		ZipFile = "${execName}-portable-v${appVersion}.zip"
	}
}

if ($deployment.WinX64 -eq $true)
{
	$distribution.WinX64 = [ordered]@{
		ExecutableName = "${assemblyName}.exe"
		TargetExecutableName = "${execName}.exe"
		TargetDirectory = Resolve-Dist-Dir -Path $publishPath -Platform $bins.WinX64
		CleanFiles = @("${assemblyName}.pdb")
		ZipFile = "${execName}-win_x64-v${appVersion}.zip"
	}
}

if ($deployment.WinX86 -eq $true)
{
	$distribution.WinX86 = [ordered]@{
		ExecutableName = "${assemblyName}.exe"
		TargetExecutableName = "${execName}.exe"
		TargetDirectory = Resolve-Dist-Dir -Path $publishPath -Platform $bins.WinX86
		CleanFiles = @("${assemblyName}.pdb")
		ZipFile = "${execName}-win_x86-v${appVersion}.zip"
	}
}

if ($deployment.LinuxArm64 -eq $true)
{
	$distribution.LinuxArm64 = [ordered]@{
		ExecutableName = "${assemblyName}"
		TargetExecutableName = "${execName}"
		TargetDirectory = Resolve-Dist-Dir -Path $publishPath -Platform $bins.LinuxArm64
		CleanFiles = @("${assemblyName}.pdb")
		ZipFile = "${execName}-linux_arm64-v${appVersion}.zip"
	}
}

if ($deployment.LinuxX64 -eq $true)
{
	$distribution.LinuxX64 = [ordered]@{
		ExecutableName = "${assemblyName}"
		TargetExecutableName = "${execName}"
		TargetDirectory = Resolve-Dist-Dir -Path $publishPath -Platform $bins.LinuxX64
		CleanFiles = @("${assemblyName}.pdb")
		ZipFile = "${execName}-linux_x64-v${appVersion}.zip"
	}
}

if ($deployment.OsxX64 -eq $true)
{
	$distribution.OsxX64 = [ordered]@{
		ExecutableName = "${assemblyName}"
		TargetExecutableName = "${execName}"
		TargetDirectory = Resolve-Dist-Dir -Path $publishPath -Platform $bins.OsxX64
		CleanFiles = @("${assemblyName}.pdb")
		ZipFile = "${execName}-osx_x64-v${appVersion}.zip"
	}
}

function Create-Release-Archice()
{
	param (
		[parameter(Mandatory = $true)]
		[Hashtable] $Config,
		[parameter(Mandatory = $true)]
		[string]$Platform
	)

	Write-Output "[STRT] Prepaing '${Platform}' build...";

	$pubPath = $Config.TargetDirectory;
	if ($Platform -ne "Portable")
	{
		$pubPath = Join-Path $pubPath "publish"
	}

	$archivePath = Join-Path $distPath $Config.ZipFile
	Remove-Item $archivePath -ErrorAction Ignore

	if (!(Test-Path $pubPath))
	{
		return;
	}

	$executableName = Join-Path $pubPath $Config.ExecutableName -ErrorAction Ignore;
	$targetExecutableName = Join-Path $pubPath $Config.ExecutableName -ErrorAction Ignore;

	if (Test-Path ($executableName))
	{
		Rename-Item $executableName $targetExecutableName
	}

	if ($Config.CleanFiles.Length -gt 0)
	{
		Write-Output "[INFO] Clearning up files...";
		foreach ($file in $Config.CleanFiles)
		{
			$targetFile = "${pubPath}/${file}";
			Remove-Item $targetFile -Recurse -ErrorAction Ignore -Force
		}
	}

	Write-Output "[INFO] Creating archive file: ${archivePath}";
	[IO.Compression.ZipFile]::CreateFromDirectory($pubPath, $archivePath, 'Fastest', $false)
	Write-Output "[DONE] Created: ${Platform}";
	Write-Output "";
}


Write-Output "[PREPAIRING] Preparing acrhive jobs";
Write-Output "";

foreach ($platform in $distribution.Keys)
{
	Create-Release-Archice -Config $distribution[$platform] -Platform $platform
}

Write-Output "[COMPLETED] All jobs are completed"