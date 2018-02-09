# Hands-on IoT

Welcome to this hands-on IoT ! 
The purpose to this lab is to focus on development basis with Rapsberry Pi 3 and Windows 10 IoT Core.

## Hardware prerequisites

To develop this solution we used the following materials :
* Raspberry Pi 3
* Micro SD card with Windows 10 IoT Core installed
* Grove Pi shield
* Temperature sensor

And of course everything required to power and interact with a raspi, it means : 
* Power supply (can be a USB power bank too)
* HDMI monitor 
* Ethernet connection to the Internet
* USB keyboard
* USB mouse (optional)


## Software prerequisites

In terms of software, you will need : 
* Windows 10 up-to-date
* Visual Studio 2017 up-to-date
* Universal Windows Platform development component, feel free to have a look on this **[link](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio)** to check your Visual Studio installation
* Windows 10 IoT Core Dashboard, downloadable **[here](http://go.microsoft.com/fwlink/?LinkID=708576)**
 
## Get the solution

Ensure to get the solution and open it with Visual Studio 2017.

 In the solution explorer view, under the References node, you will see that we are using 3 nuget packages to work : 
* GrovePi
* Microsoft.Azures.Devices.Client
* Microsoft.Devices.Tpm

## Deploy the solution to your device

We are going to deploy the solution on your raspi.
To achieve that, follw these steps : 
* power on the raspi, ensure that : the device is connected to the Internet and plugged to a monitor
* wait until Windows 10 IoT Core finished to boot 
* get the ip address of the device (you can find it on the raspi dashboard or on Windows 10 IoT Core Dashboard)
* choose the ARM mode in Visual Studio Configuration Manager
* in the solution explorer, perform a right-click on the csharp project
* in the debug tab, select "Remote Machine" as target device
* enter the ip address of the raspbi so that Visual Studio can find the targeted device in your local network
* debug the app, Visual Studio launches the deploy, targeting your device
* yeah it works !




