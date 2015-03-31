###LiveUpdate Example
This sample is to get a vision for live update of a production environment,
to get a vision how devOps can be used in production scenario,
to get feedback to the idea.

In our production scenario we have a conveyor that transport pieces of different size and count the
pieces in a first version, in the second version it counts and differs between small and big pieces.
Now an update of the software is done while the coneyor runs and the software counts and no downtime is needed.

####Inside overview
In our demo environment we have build a [fischertechnik model](http://www.fischertechnik.de/en/Home/products/industry.aspx) with one photo sensor that signals off
 when a piece interrupts the sensor. A service is connected to the hardware and provides changes of the signal. This is the FischertechnikService. 

A second service is connected to this FischertechnikService and counts the pieces. This is the CounterService.

Than a client applications displays the count. This is the ItemCounterClient.

Additional a DispatcherService is used to connect the client with the correct version of the service.

The solution also contains a new version of the CounterService which is able to decide based of the duration of the sensor interruption if it is a 
small or a big item. There is also a new version of the ItemCounterClient which shows this additional data. The goal is to include this new logic 
without stopping and restarting the application.

![components diagram](https://github.com/artiso-solutions/samples/blob/master/LiveUpdate/Diagram.png)

####How to run the sample
1. Build Solution
2. Start ServiceHost
	* to use test service run bin\ServiceHost\ConsoleHost.exe
	* to use fischertechnk real service run bin\ServiceHost\StartWithFischertechnikLogic.bat
	Service Version 1.0.0.0 is started
3. Start Client (bin\Client\Shell.exe)
	Count is displayed

#####Batch Update:
1. Leave application open and run bin\Client\ExecuteUpdate.bat
* Service is updated to Version 2.0.0.0
* Client is updated to Version 2.0.0.0 (Count, Count Big and Count Small is displayed)

#####Manual Update:
1. Copy (bin\PendingUpdates\ServiceHost\ServiceV2 to bin\ServiceHost\Service2)
	* Service Version 2.0.0.0 is started
2. Copy (bin\PendingUpdates\Client\ClientV2 to bin\Client\ClientV2)
	* Count is displayed
	* Count Big is displayed
	* Count Small is displayed 

####Automatic Fallback
1. Either click the Refresh button or wait untill Count Big or Count Small reaches 256
2. An exception is thrown and an automatic fallback to Version 1.0.0.0 will be executed on the client

####ToDos
* Fallback for service
* Activation of new features on demand 
