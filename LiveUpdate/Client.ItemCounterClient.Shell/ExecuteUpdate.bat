xcopy ..\PendingUpdates\\ServiceHost ..\ServiceHost\ /s
#sleep for 2 seconds; this works for all windows versions.
ping -n <2> 127.0.0.1 > NUL
xcopy ..\PendingUpdates\Client .\ /s
