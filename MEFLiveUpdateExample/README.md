##MEFLiveUpdateExample

Sample is used to demonstrate how an assembly can be loaded while the application is already running.<br>
To make this sample work you need to follow these steps.
* Compile solution in Visual Studio
* Go to newly created solution subfolder <b>Output\Debug</b>
* Start <b>Shell.exe</b> from folder <b>Application</b>
* A plain WPF will open that shows <b>V1</b>
* Leave application open and run <b>ExecuteUpdate.bat</b>
* Window content will change from <b>V1</b> and show <b>V2</b> instead
* Clicking on the <b>Throw Exception</b> will trigger an exception to demonstrate the fallback to the previous Version. It will automatically switch back to <b>V1</b> window content.

This is a rudimentary demonstration how it can be done to dynamically load new assembly versions at run time.
There are still some issues that need to be resolved in future.

## TODOs
* Unload old assembly versions from application

