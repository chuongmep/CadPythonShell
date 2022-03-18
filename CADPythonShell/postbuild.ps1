#Copyright 2022 Chuongmep.com ＼（＾_＾）／
param ($Configuration, $ProjectDir, $SolutionDir)
Write-Host "Solution Directory:" $SolutionDir
Write-Host "Project Directory:" $ProjectDir
Write-Host "Configuration Name Current:" $Configuration
$bundle = "C:\ProgramData\Autodesk\ApplicationPlugins\CADPythonShell.bundle\"
$content = "PackageContents.xml"
if($Configuration -match "Debug"){
	Write-Host "************Start Create Folder And Check File ＼（＾.＾）／"
	if(Test-Path $bundle){
		Write-Host "Exits Path, So Remove All File Exits"
		Remove-Item ($bundle) -Recurse
		Write-Host "Removed All File Exist"
	}
	Write-Host "************ Start Copy New File"
	xcopy ($SolutionDir + $content) $bundle /Y
	xcopy ($ProjectDir + "*.*") $bundle /Y /I /E /R
	Write-Host "************ Oh my got ! Copy Complete! Chuongmep.com ＼（＾_＾）／"
}
else
{
	Write-Host "Please Toggle To Debug Model If You Want Copy File And Debug ＼（＾_＾）／ , config in postbuild.ps1"
}

## Recommended Content
## https://docs.microsoft.com/en-us/visualstudio/ide/how-to-change-the-build-output-directory?view=vs-2022
#if you want try with post build event
#set bundle="C:\ProgramData\Autodesk\ApplicationPlugins\CADPythonShell.bundle\"
#if not exist "%bundle%" mkdir "%bundle%"
#xcopy "$(TargetDir)*.*" "%bundle%" /Y /I /E /R
#xcopy "$(SolutionDir)PackageContents.xml" "%bundle%" /Y /R