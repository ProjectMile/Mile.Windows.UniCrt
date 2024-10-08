@setlocal
@echo off

rem Change to the current folder.
cd "%~dp0"

rem Remove the output folder for a fresh compile.
rd /s /q Output

rem Initialize Visual Studio environment
call "%~dp0Mile.Project.Windows\InitializeVisualStudioEnvironment.cmd"

rem Kill all MSBuild instances to ensure the successful compilation
taskkill /f /im MSBuild.exe

rem Build all targets
MSBuild -binaryLogger:Output\BuildAllTargets.binlog -m BuildAllTargets.proj

@endlocal