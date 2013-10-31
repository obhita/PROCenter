@procenter Copyright 2013 FEI. All rights reserved.
@procenter For information about IIS Web Deploy technology,
@procenter please visit http://go.microsoft.com/?linkid=9278654
@procenter ---------------------------------------------------------------------------------
@if %_echo%!==! echo off
setlocal
@procenter ---------------------------------------------------------------------------------
@procenter Please Make sure you have Web Deploy install in your machine. 
@procenter Alternatively, you can explicit set the MsDeployPath to the location it is on your machine
@procenter set MSDeployPath="C:\Program Files (x86)\IIS\Microsoft Web Deploy\"
@procenter ---------------------------------------------------------------------------------
                      
@procenter ---------------------------------------------------------------------------------
@procenter if user does not set MsDeployPath environment variable, we will try to retrieve it from registry.
@procenter ---------------------------------------------------------------------------------
if "%MSDeployPath%" == "" (
for /F "usebackq tokens=1,2,*" %%h  in (`reg query "HKLM\SOFTWARE\Microsoft\IIS Extensions\MSDeploy" /s  ^| findstr -i "InstallPath"`) do (
if /I "%%h" == "InstallPath" ( 
if /I "%%i" == "REG_SZ" ( 
if not "%%j" == "" ( 
if "%%~dpj" == "%%j" ( 
set MSDeployPath=%%j
))))))

if not exist "%MSDeployPath%\msdeploy.exe" (
echo. msdeploy.exe is not found on this machine. Please install Web Deploy before execute the script. 
echo. Please visit http://go.microsoft.com/?linkid=9278654
goto :usage
)

set RootPath=%~dp0
set _ArgTestDeploy=
set _ArgDestinationType=
set _ArgComputerName=
set _ArgUserName=
set _ArgPassword=
set _ArgEncryptPassword=
set _ArgIncludeAcls=False
set _ArgAuthType=
set _ArgtempAgent=
set _ArgLocalIIS=
set _ArgLocalIISVersion=
                      
                      
@procenter ---------------------------------------------------------------------------------
@procenter Simple Parse the arguments
@procenter ---------------------------------------------------------------------------------
:NextArgument
set _ArgCurrentOriginal=%1
set _ArgCurrent=%~1

if /I "%_ArgCurrent%" == "" goto :GetStarted
if /I "%_ArgCurrent%" == "/T" set _ArgTestDeploy=true&goto :ArgumentOK
if /I "%_ArgCurrent%" == "/Y" set _ArgTestDeploy=false&goto :ArgumentOK

set _ArgFlag=%_ArgCurrent:~0,3%
set _ArgValue=%_ArgCurrent:~3%

if /I "%_ArgFlag%" == "/M:" set _ArgComputerName=%_ArgValue%&goto :ArgumentOK
if /I "%_ArgFlag%" == "/U:" set _ArgUserName=%_ArgValue%&goto :ArgumentOK
if /I "%_ArgFlag%" == "/P:" set _ArgPassword=%_ArgValue%&goto :ArgumentOK
if /I "%_ArgFlag%" == "/E:" set _ArgEncryptPassword=%_ArgValue%&goto :ArgumentOK
if /I "%_ArgFlag%" == "/I:" set _ArgIncludeAcls=%_ArgValue%&goto :ArgumentOK
if /I "%_ArgFlag%" == "/A:" set _ArgAuthType=%_ArgValue%&goto :ArgumentOK
if /I "%_ArgFlag%" == "/G:" set _ArgtempAgent=%_ArgValue%&goto :ArgumentOK
if /I "%_ArgFlag%" == "/D:" set _ArgDestinationType=%_ArgValue%&goto :ArgumentOK

@procenter Any addition flags, pass through to the msdeploy
set _ArgMsDeployAdditionalFlags=%_ArgMsDeployAdditionalFlags% %_ArgCurrentOriginal%

:ArgumentOK
shift
goto :NextArgument

:GetStarted
if /I "%_ArgTestDeploy%" == "" goto :usage
if /I "%_ArgDestinationType%" == ""  goto :usage

set _Destination=%_ArgDestinationType%
if not "%_ArgComputerName%" == "" set _Destination=%_Destination%,computerName='%_ArgComputerName%'
if not "%_ArgUserName%" == "" set _Destination=%_Destination%,userName='%_ArgUserName%'
if not "%_ArgPassword%" == "" set _Destination=%_Destination%,password='%_ArgPassword%'
if not "%_ArgAuthType%" == "" set _Destination=%_Destination%,authtype='%_ArgAuthType%'
if not "%_ArgEncryptPassword%" == "" set _Destination=%_Destination%,encryptPassword='%_ArgEncryptPassword%'
if not "%_ArgIncludeAcls%" == "" set _Destination=%_Destination%,includeAcls='%_ArgIncludeAcls%'
if not "%_ArgtempAgent%" == "" set _Destination=%_Destination%,tempAgent='%_ArgtempAgent%'

@procenter ---------------------------------------------------------------------------------
@procenter add -whatif when -T is specified                      
@procenter ---------------------------------------------------------------------------------
if /I "%_ArgTestDeploy%" NEQ "false" (
set _MsDeployAdditionalFlags=%_MsDeployAdditionalFlags% -whatif
)

@procenter ---------------------------------------------------------------------------------
@procenter pass through the addition msdeploy.exe Flags
@procenter ---------------------------------------------------------------------------------
set _MsDeployAdditionalFlags=%_MsDeployAdditionalFlags% %_ArgMsDeployAdditionalFlags%

@procenter ---------------------------------------------------------------------------------
@procenter Execute msdeploy.exe command line
@procenter ---------------------------------------------------------------------------------
echo. Start executing msdeploy.exe
echo -------------------------------------------------------

echo. "%MSDeployPath%\msdeploy.exe" -preSync:runCommand='net stop "PROCenter Job Scheduler"',waitAttempts=60 -source:dirPath='%RootPath%' -dest:dirPath='%_Destination%' -verb:sync %_MsDeployAdditionalFlags% -postSync:runCommand="%_ArgDestinationType%\InstallAndStartProCenterJobScheduler.bat",waitAttempts=60
"%MSDeployPath%\msdeploy.exe" -preSync:runCommand="%_ArgDestinationType%\UninstallProCenterJobScheduler.bat",waitAttempts=60 -source:dirPath='%RootPath%' -dest:dirPath=%_Destination% -verb:sync %_MsDeployAdditionalFlags% -postSync:runCommand="%_ArgDestinationType%\InstallAndStartProCenterJobScheduler.bat",waitAttempts=60
goto :eof

@procenter ---------------------------------------------------------------------------------
@procenter Usage
@procenter ---------------------------------------------------------------------------------
:usage
echo =========================================================
if not exist "%RootPath%ProCenter.Mvc.deploy-readme.txt" (
echo Usage:%~nx0 [/T^|/Y] [/M:ComputerName] [/U:userName] [/P:password] [/G:tempAgent] [additional msdeploy flags ...]
echo Required flags:
echo /T  Calls msdeploy.exe with the "-whatif" flag, which simulates deployment. 
echo /Y  Calls msdeploy.exe without the "-whatif" flag, which deploys the package to the current machine or destination server 
echo Optional flags:  
echo. By Default, this script deploy to the current machine where this script is invoked which will use current user credential without tempAgent. 
echo.   Only pass these arguments when in advance scenario.
echo /M:  Msdeploy destination name of remote computer or proxy-URL. Default is local.
echo /U:  Msdeploy destination user name. 
echo /P:  Msdeploy destination password.
echo /G:  Msdeploy destination tempAgent. True or False. Default is false.
echo /A:  specifies the type of authentication to be used. The possible values are NTLM and Basic. If the wmsvc provider setting is specified, the default authentication type is Basic
otherwise, the default authentication type is NTLM.
echo /L:  Deploy to Local IISExpress User Instance.  

echo.[additional msdeploy flags]: note: " is required for passing = through command line.
echo  "-skip:objectName=setAcl" "-skip:objectName=dbFullSql"
echo.Alternative environment variable _MsDeployAdditionalFlags is also honored.
echo.
echo. Please make sure MSDeploy is installed in the box http://go.microsoft.com/?linkid=9278654
echo.
echo In addition, you can change IIS Application Name, Physical path, 
echo connectionString and other deploy parameters in the following file:
echo "%_DeploySetParametersFile%"
echo.
echo For more information about this batch file, visit http://go.microsoft.com/fwlink/?LinkID=183544 
) else (
start notepad "%RootPath%ProCenter.Mvc.deploy-readme.txt"
)
echo =========================================================