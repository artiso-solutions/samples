###LiveUpdate Example
This sample is to get a vision for live update of a production environment,
to get a vision how devOps can be used in production scenario,
to get feedback to the idea.

In our production scenario we have a conveyor that transport pieces of different size and count the
pieces in a first version, in the second version it counts and differs between small and big pieces.
Now an update of the software is done while the coneyor runs and the software counts and no downtime is needed.

####Inside overview
In our demo environment we have build a [fischertechnik model](http://www.fischertechnik.de/en/Home/products/industry.aspx) with one photo sensor that signals off
 when a piece interrupts the sensor and on if no piece is between the sensor. A service is connected to 
the hardware and provides changes of the signal. This is the FischertechnikService. 

A second service is connected to this FischertechnikService and counts the pieces. This is the CounterService.

Than a client applications displays the count. This is the ItemCounterClient.

Additional a DispatcherService is used to connect the client with the correct version of the service.


####How to run the sample
1. Build Solution
2. Start ServiceHost (bin\ServiceHost\ConsoleHost.exe)
    (if the application is started with an argument a test service is used, otherwise a real fischertechnik service.)
	Service Version 1.0.0.0 is started
3. Start Client (bin\Client\Shell.exe)
	Count is displayed

#####Update:
1. Copy (bin\ServiceV2 to bin\ServiceHost\Service2)
	Service Version 2.0.0.0 is started
2. Copy (bin\ClientV2 to bin\Client\ClientV2)
	Count is displayed
	Count Big is displayed
	Count Small is displayed 

####ToDos
* Fallback to previous version if errors occurs

* Activation of new features on demand 
