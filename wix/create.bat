del installer.wixobj
"%wix%\bin\candle" installer.wxs -ext WiXNetFxExtension -ext WixDifxAppExtension -ext WixUIExtension.dll -ext WixUtilExtension -ext WixIisExtension
"%wix%\bin\light" installer.wixobj "%wix%\bin\difxapp_x86.wixlib" -o MissionPlanner-1.3.43.msi -ext WiXNetFxExtension -ext WixDifxAppExtension -ext WixUIExtension.dll -ext WixUtilExtension -ext WixIisExtension
pause
"C:\Program Files\7-Zip\7z.exe" a -tzip -xr!*.log -xr!*.log* -xr!beta.bat -xr!cameras.xml -xr!firmware.hex -xr!*.zip -xr!stats.xml -xr!*.bin -xr!*.xyz -xr!*.sqlite -xr!*.dxf -xr!*.zip -xr!*.h -xr!*.param -xr!ParameterMetaData.xml -xr!translation -xr!mavelous_web -xr!stats.xml -xr!driver -xr!*.etag -xr!srtm -xr!*.rlog -xr!*.zip -xr!*.tlog -xr!config.xml -xr!gmapcache -xr!eeprom.bin -xr!dataflash.bin -xr!*.new -xr!*.log -xr!ArdupilotPlanner.log* -xr!cameras.xml -xr!firmware.hex -xr!*.zip -xr!stats.xml -xr!ParameterMetaData.xml -xr!*.etag -xr!*.rlog -xr!*.tlog -xr!config.xml -xr!gmapcache -xr!eeprom.bin -xr!dataflash.bin -xr!*.new MissionPlanner-1.3.43.zip ..\bin\release\*
About to upload!!!!!!!!!
pause
c:\cygwin\bin\ln.exe -f -s MissionPlanner-1.3.43.zip MissionPlanner-latest.zip
c:\cygwin\bin\ln.exe -f -s MissionPlanner-1.3.43.msi MissionPlanner-latest.msi
c:\cygwin\bin\rsync.exe -Pv -e '/usr/bin/ssh -o StrictHostKeyChecking=no -o UserKnownHostsFile=/dev/null -i /cygdrive/c/Users/michael/sitl' MissionPlanner-1.3.43.zip michael@bios.ardupilot.org:MissionPlanner/
c:\cygwin\bin\rsync.exe -Pv -e '/usr/bin/ssh -o StrictHostKeyChecking=no -o UserKnownHostsFile=/dev/null -i /cygdrive/c/Users/michael/sitl' MissionPlanner-1.3.43.msi michael@bios.ardupilot.org:MissionPlanner/
c:\cygwin\bin\rsync.exe -Pv -e '/usr/bin/ssh -o StrictHostKeyChecking=no -o UserKnownHostsFile=/dev/null -i /cygdrive/c/Users/michael/sitl'   -l MissionPlanner-latest.zip michael@bios.ardupilot.org:MissionPlanner/
c:\cygwin\bin\rsync.exe -Pv -e '/usr/bin/ssh -o StrictHostKeyChecking=no -o UserKnownHostsFile=/dev/null -i /cygdrive/c/Users/michael/sitl'   -l MissionPlanner-latest.msi michael@bios.ardupilot.org:MissionPlanner/
