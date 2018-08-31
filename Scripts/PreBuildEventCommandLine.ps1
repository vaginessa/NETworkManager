param(
    [Parameter(
        Position=0,
        Mandatory=$true)]
    [String]$OutPath
)

# Fix wrong path
# Quotation marks are required if a blank is in the path... If there is no blank space in the path, a quote will be add to the end...
# VS prebuild event call: PowerShell.exe -ExecutionPolicy Bypass -NoProfile -File "$(ProjectDir)..\..\Scripts\PreBuildEventCommandLine.ps1" "$(TargetDir)"
if(-not($OutPath.StartsWith('"')))
{
    $OutPath = $OutPath.TrimEnd('"')
}

# Test if files are already there...
if((Test-Path -Path "$OutPath\MSTSCLib.dll") -and (Test-Path -Path "$OutPath\AxMSTSCLib.dll"))
{
    Write-Host "MSTSCLib.dll and AxMSTSCLib.dll already there!"
    return
}

# x86 or x64
$ProgramFiles_Path = ${Env:ProgramFiles(x86)}

if([String]::IsNullOrEmpty($ProgramFiles_Path))
{
    $ProgramFiles_Path = $Env:ProgramFiles    
}

# Get aximp from sdk
$AximpPath = ((Get-ChildItem -Path "$ProgramFiles_Path\Microsoft SDKs\Windows" -Recurse -Filter "aximp.exe" -File) | Sort-Object FullName | Select-Object -First 1).FullName

if([String]::IsNullOrEmpty($AximpPath))
{
    Write-Host "Aximp.exe not found on this system!"
    return
}
else 
{
    Write-Host "Using aximp.exe from: $AximpPath"    
}

# Change location
Write-Host "Change location to: $OutPath"
Set-Location -Path $OutPath 

# Create MSTSCLib.dll and AxMSTSCLib.dll
Write-Host "Build MSTSCLib.dll and AxMSTSCLib.dll ..."
Start-Process -FilePath $AximpPath -ArgumentList "$($Env:windir)\system32\mstscax.dll" -Wait -NoNewWindow
