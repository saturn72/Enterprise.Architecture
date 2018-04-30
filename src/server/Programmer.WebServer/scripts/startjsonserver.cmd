SET procToKillName=cmd.exe
SET workDir=%~dp0..\..\..\db
SET dbFile=db.json
SET port=3000

REM ECHO Kill all cmd processes
REM taskkill /F /IM %procToKillName% /T
REM timeout /t 1


ECHO Start json-server in port %port% using file - %dbFile%

ECHO Working directory: %workDir%

SET cmdParameters=-w %dbFile% -p %port%
ECHO Command Parameters: %cmdParameters%

SET startJsonServerCmd="json-server http://localhost:3000" /D %workDir% "json-server" %cmdParameters%
START %startJsonServerCmd%
