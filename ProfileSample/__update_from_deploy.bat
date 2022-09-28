@echo off
set PfAppData=C:\Users\...

call :update nlog.config
call :update pumpservice.address.txt

md PumpStationParams
call :update PumpStationParams\groups.json
call :update PumpStationParams\params.json
call :update PumpStationParams\schemas.json
call :update PumpStationParams\stations.json

exit /b

:update
echo Copying %1...
copy "%PfAppData%\%1" ".\%1"
exit /b
