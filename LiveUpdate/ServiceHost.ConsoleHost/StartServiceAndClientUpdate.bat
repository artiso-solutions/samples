xcopy .\PendingUpdates\\ServiceHost .\ServiceHost\ /s /y
#sleep for 2 seconds; this works for all windows versions.
ping -n <2> 127.0.0.1 > NUL
xcopy .\PendingUpdates\Client .\Client\ /s /y
