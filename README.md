# PCR.Android.Client
A simple android client that in time will allow remote control of some basic functions of a PC. Currently only handles volume mixer control

## Getting Started
Download the PCR.Client.Android project files and also a copy of the PCR.Common project files, ensure both projects are in the same directory. Open the visual studio project for PCR.Client.Android and ensure all nuget packages are installed and verfied, and the project was able to find the PCR.Common project. Rebuild the project and then package it for release using visual studio xamarins in built archive functionality.

## Deployment
Side load the apk onto the device and start the application once the installation is completed. When the app opens enter a PCR server IP address to connect you device to your server PC.
