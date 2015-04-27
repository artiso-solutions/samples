start "" ".\ServiceHost\ConsoleHost.exe" "--UseFischerTechnikController"
#sleep for 4 seconds; this works for all windows versions.
ping -n <4> 127.0.0.1 > NUL
start "" ".\Client\Shell.exe"
#sleep for 2 seconds; this works for all windows versions.
ping -n <2> 127.0.0.1 > NUL
